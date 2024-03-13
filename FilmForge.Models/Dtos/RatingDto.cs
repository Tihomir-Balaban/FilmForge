namespace FilmForge.Models.Dtos;

public class RatingDto
{
    public int Id { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime ModifiedOn { get; set; }
    public int Value { get; set; }
    public int MovieId { get; set; }
    public int UserId { get; set; }
}
