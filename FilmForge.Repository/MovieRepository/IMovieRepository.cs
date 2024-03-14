namespace FilmForge.Repository.MovieRepository;

public interface IMovieRepository : IGenericRepository<MovieDto, Movie>
{
    Task<MovieDto[]> GetByDirectorIdAsync(int directorId);
}
