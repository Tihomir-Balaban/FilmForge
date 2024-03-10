using FilmForge.Models.Enum;

namespace FilmForge.Models.Dtos;

public class UserDto
{
    public int Id { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime ModifiedOn { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Salt { get; set; }
    public UserRole Role { get; set; }
}
