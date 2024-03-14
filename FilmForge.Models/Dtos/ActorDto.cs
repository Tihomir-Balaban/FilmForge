namespace FilmForge.Models.Dtos;

public class ActorDto
{
    public int Id { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime ModifiedOn { get; set; }
    public string Name { get; set; }
    public string Bio { get; set; }
    public ulong Fee { get; set; }
    public int UserId { get; set; }
    public ICollection<MovieDto> Movies { get; set; }
}
