
using FilmForge.Models.Dto;

namespace Service.Security;

public interface ISecurityService
{
    (byte[] PasswordHash, byte[] Salt) HashPassword(string password);
    bool VerifyPassword(string password, byte[] storedHash, byte[] storedSalt);
    string JwtSecurityHandler(UserDto user);
}