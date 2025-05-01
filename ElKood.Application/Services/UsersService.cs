using AutoMapper;
using ElKood.Application.Configuration;
using ElKood.Application.Interfaces;
using ElKood.Application.Shared.Models.Dtos.Identity;
using ElKood.Application.Shared.Models.Inputs.Identity;
using ElKood.Core.Abstractions;
using ElKood.Core.Entities.Identity;
using ElKood.Core.Enums;
using ElKood.Core.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ElKood.Application.Services
{
    public class UsersService : IUsersService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly Jwt _jwt;
        private readonly IAppLogService _appLogService;
        private readonly IMapper _mapper;
        private readonly IValidator<LoginInput> _loginInputValidator;
        private readonly IValidator<RegistrationInput> _registrationInputValidator;
        private readonly IUnitOfWork _unitOfWork;

        public UsersService(UserManager<ApplicationUser> userManager,
                          IOptions<Jwt> jwt,
                          IAppLogService appLogService,
                          IMapper mapper,
                          IValidator<LoginInput> loginInputValidator,
                          IValidator<RegistrationInput> registrationInputValidator,
                          IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _jwt = jwt.Value;
            _appLogService = appLogService;
            _mapper = mapper;
            _loginInputValidator = loginInputValidator;
            _registrationInputValidator = registrationInputValidator;
            _unitOfWork = unitOfWork;
        }

        public async Task AddUserAsync(RegistrationInput registrationInput)
        {
            var registrationValidationResult = _registrationInputValidator.Validate(registrationInput);
            if (!registrationValidationResult.IsValid)
            {
                throw new ValidationException(registrationValidationResult.Errors);
            }
            var userWithSameEmail = await _userManager.FindByEmailAsync(registrationInput.Email);
            if (userWithSameEmail != null)
            {
                throw new UserFriendlyException("User already exists with same Email.", HttpStatusCode.BadRequest);
            }

            var userWithSameUsername = await _userManager.FindByNameAsync(registrationInput.UserName);
            if (userWithSameUsername != null)
            {
                throw new UserFriendlyException("User already exists with same Username.", HttpStatusCode.BadRequest);
            }

            var user = new ApplicationUser
            {
                UserName = registrationInput.UserName,
                Email = registrationInput.Email
            };
            var result = await _userManager.CreateAsync(user, registrationInput.Password);
            if (result.Succeeded)
            {

                await _userManager.AddToRoleAsync(user, registrationInput.Role);
                var userDto = new UserDto();
                userDto.Id = user.Id;
                userDto.UserName = user.UserName;
                userDto.Email = user.Email;
                userDto.Roles = await _userManager.GetRolesAsync(user);
            }
            else if (result.Errors.Any(x => x.Code.Contains("Password")))
            {
                List<string> errorCodes = new List<string>();
                foreach (var error in result.Errors)
                {
                    errorCodes.Add(error.Code);
                }
                throw new UserFriendlyException(string.Join(',', errorCodes), HttpStatusCode.BadRequest);
            }

            throw new UserFriendlyException("User could not be created.", HttpStatusCode.BadRequest);
        }

        public List<UserDto> GetAllUsers()
        {
            var users = _unitOfWork.Users.GetAll();
            return _mapper.Map<List<UserDto>>(users);
        }

        public async Task<AuthenticationDto> GetRefreshTokenAsync(string token)
        {
            var authenticationDto = new AuthenticationDto();
            var user = _unitOfWork.Users.GetAll().SingleOrDefault(u => u.RefreshTokens.Any(t => t.Token == token));
            if (user == null)
            {
                throw new UserFriendlyException("Token did not match any user.", HttpStatusCode.Unauthorized);
            }

            var refreshToken = user.RefreshTokens.Single(x => x.Token == token);

            if (!refreshToken.IsActive)
            {
                throw new UserFriendlyException("Token is not active.", HttpStatusCode.Unauthorized);
            }

            //Revoke Current Refresh Token
            refreshToken.Revoked = DateTime.UtcNow;

            //Generate new Refresh Token and save to Database
            var newRefreshToken = GenerateRefreshToken();
            user.RefreshTokens.Add(newRefreshToken);
            await _unitOfWork.Users.UpdateAsync(user);
            await _unitOfWork.SaveChangesAsync();

            //Generates new jwt
            authenticationDto.IsAuthenticated = true;

            JwtSecurityToken jwtSecurityToken = await CreateJwtToken(user);
            authenticationDto.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            authenticationDto.UserName = user.UserName;
            var rolesList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
            authenticationDto.Roles = rolesList.ToList();
            authenticationDto.TokenDurationM = _jwt.DurationInMinutes;
            authenticationDto.RefreshToken = newRefreshToken.Token;
            authenticationDto.RefreshTokenDurationM = 10 * 24 * 60;
            authenticationDto.RefreshTokenExpiry = newRefreshToken.Expires;

            return authenticationDto;
        }

        public UserDto GetById(string id)
        {
            var user = _unitOfWork.Users.GetAll().FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                throw new UserFriendlyException("User not found.", HttpStatusCode.NotFound);
            }
            return _mapper.Map<UserDto>(user);
        }

        public async Task<AuthenticationDto> LoginUserAsync(LoginInput loginInput)
        {
            var authenticationDto = new AuthenticationDto();

            var loginValidationResult = _loginInputValidator.Validate(loginInput);
            if (!loginValidationResult.IsValid)
            {
                throw new ValidationException(loginValidationResult.Errors);
            }

            var user = await _userManager.FindByNameAsync(loginInput.UserName);
            if (user == null)
            {
                await _appLogService.LogEvent(LogEventTypes.Logging, loginInput.UserName, "Username not found.");
                throw new UserFriendlyException("Username not found.", HttpStatusCode.NotFound);
            }

            if (await _userManager.CheckPasswordAsync(user, loginInput.Password))
            {
                authenticationDto.IsAuthenticated = true;

                authenticationDto.UserName = user.UserName;
                var rolesList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
                authenticationDto.Roles = rolesList.ToList();
                JwtSecurityToken jwtSecurityToken = await CreateJwtToken(user);
                authenticationDto.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
                authenticationDto.TokenDurationM = _jwt.DurationInMinutes;
                authenticationDto.TokenExpiry = jwtSecurityToken.ValidTo;
                if (user.RefreshTokens.Any(a => a.IsActive))
                {
                    var activeRefreshToken = user.RefreshTokens.Where(a => a.IsActive == true).FirstOrDefault();
                    authenticationDto.RefreshToken = activeRefreshToken.Token;
                    //Static Value
                    authenticationDto.RefreshTokenDurationM = 10 * 24 * 60;
                    authenticationDto.RefreshTokenExpiry = activeRefreshToken.Expires;
                }
                else
                {
                    var refreshToken = GenerateRefreshToken();
                    authenticationDto.RefreshToken = refreshToken.Token;
                    //Static Value
                    authenticationDto.RefreshTokenDurationM = 10 * 24 * 60;
                    authenticationDto.RefreshTokenExpiry = refreshToken.Expires;
                    user.RefreshTokens.Add(refreshToken);
                    await _unitOfWork.Users.UpdateAsync(user);
                    await _unitOfWork.SaveChangesAsync();
                }
                await _appLogService.LogEvent(LogEventTypes.Logging, authenticationDto.UserName, "User Logged-in Successfully.");
                return authenticationDto;

            }
            authenticationDto.IsAuthenticated = false;

            await _appLogService.LogEvent(LogEventTypes.Logging, authenticationDto.UserName, $"Incorrect Credentials for user {user.UserName}.");
            throw new UserFriendlyException($"Incorrect Credentials for user {user.UserName}.", HttpStatusCode.BadRequest);
        }

        private async Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();
            foreach (var role in roles)
            {
                roleClaims.Add(new Claim("roles", role));
            }
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id),
                new Claim(ClaimTypes.Name, user.UserName)
            }
            .Union(roleClaims)
            .Union(userClaims);
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwt.DurationInMinutes),
                signingCredentials: signingCredentials);
            return jwtSecurityToken;
        }

        private static RefreshToken GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            RandomNumberGenerator.Create().GetBytes(randomNumber);
            return new RefreshToken
            {
                Token = Convert.ToBase64String(randomNumber),
                Expires = DateTime.UtcNow.AddDays(10),
                Created = DateTime.UtcNow
            };

        }
    }
}
