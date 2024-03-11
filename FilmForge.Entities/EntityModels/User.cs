using FilmForge.Entities.EntityModels.Base;
using FilmForge.Common.Enum;

namespace FilmForge.Entities.EntityModels;

public class User : BaseEntity
{
    public string Name { get; set; }
    public string Email { get; set; }
    public byte[] Password{ get; set; }
    public byte[] Salt { get; set; }
    public UserRole Role { get; set; }
}
