using ElKood.Application.Shared.Models.Dtos.Identity;
using ElKood.Application.Shared.Models.Inputs.Identity;

namespace ElKood.Application.Interfaces
{
    public interface IUsersService
    {
        UserDto GetById(string id);

        List<UserDto> GetAllUsers();

        Task<AuthenticationDto> LoginUserAsync(LoginInput model);

        Task<AuthenticationDto> GetRefreshTokenAsync(string token);

        Task AddUserAsync(RegistrationInput model);
    }
}
