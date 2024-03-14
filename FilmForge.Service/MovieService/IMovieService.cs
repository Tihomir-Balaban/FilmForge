namespace FilmForge.Service.MovieService;

public interface IMovieService : IGenericService<MovieDto>
{
    Task<MovieDto[]> GetByDirectorIdAsync(int directorId);
}
