using FilmForge.Entities.EntityModels.Base;

namespace FilmForge.Entities.EntityModels;

public class Genre : BaseEntity
{
    public string Name { get; set; }
    public ICollection<Movie> Movies { get; set; }
}