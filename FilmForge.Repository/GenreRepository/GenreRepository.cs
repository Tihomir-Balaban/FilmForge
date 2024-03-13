namespace FilmForge.Repository.GenreRepository;

public class GenreRepository : IGenreRepository
{
    public Task<GenreDto> CreateAsync(Genre entity, GenreDto dto)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<GenreDto[]> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<GenreDto> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<GenreDto> UpdateAsync(int id, Genre entity)
    {
        throw new NotImplementedException();
    }
}