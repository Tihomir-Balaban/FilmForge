namespace FilmForge.Service.MovieService;

public class MovieService : IMovieService
{
    public Task<MovieDto> CreateAsync(MovieDto dto)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<MovieDto[]> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<MovieDto> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<MovieDto> UpdateAsync(int id, MovieDto dto)
    {
        throw new NotImplementedException();
    }
}