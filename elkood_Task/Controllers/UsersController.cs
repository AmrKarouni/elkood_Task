using ElKood.Application.Interfaces;
using ElKood.Application.Shared.Models.Inputs.Identity;
using ElKood.Core.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace elkood_Task.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _userService;

        public UsersController(IUsersService userService)
        {
            _userService = userService;
        }

        [Authorize(Roles = "Owner")]
        [HttpPost("add-user")]
        public async Task<IActionResult> AddUser(RegistrationInput model)
        {
            try
            {
                await _userService.AddUserAsync(model);
                return Created();
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (UserFriendlyException ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("login-user")]
        public async Task<IActionResult> LoginUser(LoginInput loginInput)
        {
            try
            {
                var result = await _userService.LoginUserAsync(loginInput);
                return Ok(result);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (UserFriendlyException ex)
            {
                if (ex.Code == System.Net.HttpStatusCode.NotFound)
                {
                    return NotFound(ex.Message);
                }
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Owner,Guest")]
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken(string token)
        {
            try
            {
                var result = await _userService.GetRefreshTokenAsync(token);
                return Ok(result);
            }
            catch (UserFriendlyException ex)
            {
                if (ex.Code == System.Net.HttpStatusCode.Unauthorized)
                {
                    return Unauthorized(ex.Message);
                }

                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Owner")]
        [HttpGet("get-all-users")]
        public IActionResult GetAllUsers()
        {
            var result = _userService.GetAllUsers();
            return Ok(result);
        }

        [Authorize(Roles = "Owner")]
        [HttpGet("get-user/{id}")]
        public IActionResult GetById(string id)
        {
            try
            {
                var result = _userService.GetById(id);
                return Ok(result);
            }
            catch (UserFriendlyException ex)
            {
                if (ex.Code == System.Net.HttpStatusCode.NotFound)
                {
                    return NotFound(ex.Message);
                }
                return BadRequest(ex.Message);
            }

        }

    }
}
