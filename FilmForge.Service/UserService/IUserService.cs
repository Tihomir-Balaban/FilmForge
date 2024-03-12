using FilmForge.Models.Utility;

namespace FilmForge.Service.UserService;

public interface IUserService : IGenericService<UserDto>
{
    Task<UserDto> GetUserByEmailAndPassord(string email, string password);
    Task<UserDto> LoginUserAsync(LoginRequest login);
}
