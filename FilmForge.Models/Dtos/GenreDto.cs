namespace FilmForge.Models.Dtos;

public class GenreDto
{
    public int Id { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime ModifiedOn { get; set; }
    public string Name { get; set; }
    public ICollection<MovieDto> Movies { get; set; }
}
