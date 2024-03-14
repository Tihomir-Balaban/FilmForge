namespace FilmForge.Models.Dtos;

public class MovieDto
{
    public int Id { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime ModifiedOn { get; set; }
    public string Title { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime ReleaseDate { get; set; }
    public ulong Budget { get; set; }
    public int GenreId { get; set; }
    public int DirectorId { get; set; }
    public ICollection<ActorDto> Actors { get; set; }
}
