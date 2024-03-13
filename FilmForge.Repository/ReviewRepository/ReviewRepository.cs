namespace FilmForge.Repository.ReviewRepository;

public class ReviewRepository : IReviewRepository
{
    public Task<ReviewDto> CreateAsync(Review entity, ReviewDto dto)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<ReviewDto[]> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<ReviewDto> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<ReviewDto> UpdateAsync(int id, Review entity)
    {
        throw new NotImplementedException();
    }
}