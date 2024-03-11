using FilmForge.Entities.EntityModels;
using FilmForge.Models.Dtos;
using FilmForge.Repository.Generics;

namespace FilmForge.Repository.UserRepository;

public interface IUserRepository : IGenericRepository<UserDto, User>
{
    Task<UserDto> GetUserByEmailAndPassord(string email, string password);
}
