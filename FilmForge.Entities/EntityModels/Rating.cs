using FilmForge.Entities.EntityModels.Base;
using System.ComponentModel.DataAnnotations;

namespace FilmForge.Entities.EntityModels;

public class Rating : BaseEntity
{
    [Range(1, 5)]
    public int Value { get; set; }

    public int MovieId { get; set; }
    public Movie Movie { get; set; }

    public int UserId { get; set; }
    public User User { get; set; }
}
