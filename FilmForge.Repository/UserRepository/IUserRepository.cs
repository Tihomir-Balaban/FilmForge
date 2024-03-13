namespace FilmForge.Repository.UserRepository;

public interface IUserRepository : IGenericRepository<UserDto, User>
{
    Task<UserDto> GetUserByEmailAndPassord(string email, string password);
}
