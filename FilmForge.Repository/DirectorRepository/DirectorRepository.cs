namespace FilmForge.Repository.DirectorRepository;

public class DirectorRepository : IDirectorRepository
{
    public Task<DirectorDto> CreateAsync(Director entity, DirectorDto dto)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<DirectorDto[]> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<DirectorDto> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<DirectorDto> UpdateAsync(int id, Director entity)
    {
        throw new NotImplementedException();
    }
}