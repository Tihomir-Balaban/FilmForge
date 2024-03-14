using FilmForge.Entities.EntityModels.Base;

namespace FilmForge.Entities.EntityModels;

public class Movie : BaseEntity
{
    public string Title { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime ReleaseDate { get; set; }
    public ulong Budget { get; set; }

    public int GenreId { get; set; }
    public Genre Genre { get; set; } 

    public int DirectorId { get; set; }
    public Director Director { get; set; }

    public ICollection<Actor> Actors { get; set; }
}
