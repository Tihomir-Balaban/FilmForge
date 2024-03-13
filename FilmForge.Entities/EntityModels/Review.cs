using FilmForge.Entities.EntityModels.Base;
using System.ComponentModel.DataAnnotations;

namespace FilmForge.Entities.EntityModels;

public class Review : BaseEntity
{
    public string Title { get; set; }

    public string Content { get; set; }

    public int MovieId { get; set; }
    public Movie Movie { get; set; }

    public int UserId { get; set; }
    public User User { get; set; }
}
