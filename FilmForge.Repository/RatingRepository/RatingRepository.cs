namespace FilmForge.Repository.RatingRepository;

public class RatingRepository : IRatingRepository
{
    public Task<RatingDto> CreateAsync(Rating entity, RatingDto dto)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<RatingDto[]> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<RatingDto> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<RatingDto> UpdateAsync(int id, Rating entity)
    {
        throw new NotImplementedException();
    }
}