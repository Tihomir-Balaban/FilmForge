using FilmForge.Models.Dtos;
using D = FilmForge.Models.Dtos;

namespace FilmForge.Models.Statics.MovieDto;

public static class MovieDtoHelpers
{
    public static bool CheckMovieBugeting(this D.MovieDto movieDto)
    {
        return movieDto.Budget >= movieDto.Actors
            .Select(a => a.Fee)
            .Aggregate(0UL, (current, number) => current + number);
    }
}
