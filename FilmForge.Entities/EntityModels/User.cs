using FilmForge.Entities.EntityModels.Base;
using FilmForge.Models.Enum;

namespace FilmForge.Entities.EntityModels;

public class User : BaseEntity
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password{ get; set; }
    public string Salt { get; set; }
    public UserRole Role { get; set; }
}
