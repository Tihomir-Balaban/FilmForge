namespace FilmForge.Service.UserService;

public interface IUserService : IGenericService<UserDto>
{
    Task<UserDto> GetUserByEmailAndPassord(string email, string password);
}
