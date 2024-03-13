namespace FilmForge.Models.Dtos;

public class ReviewDto
{
    public int Id { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime ModifiedOn { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public int MovieId { get; set; }
    public int UserId { get; set; }
}