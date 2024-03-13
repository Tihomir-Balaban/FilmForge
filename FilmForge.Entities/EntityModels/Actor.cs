using FilmForge.Entities.EntityModels.Base;

namespace FilmForge.Entities.EntityModels;

public class Actor : BaseEntity
{
    public string Name { get; set; }
    public string Bio { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }

    public ICollection<Movie> Movies { get; set; }
}
