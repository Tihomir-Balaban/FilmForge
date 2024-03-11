using FilmForge.Models.Dtos;
using FilmForge.Repository.Generics;

namespace FilmForge.Service.UserService;

public interface IUserService : IGenericService<UserDto>
{
    Task<UserDto> GetUserByEmailAndPassord(string email, string password);
}
