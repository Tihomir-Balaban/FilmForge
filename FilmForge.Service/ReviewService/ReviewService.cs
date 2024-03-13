using FilmForge.Repository.ReviewRepository;

namespace FilmForge.Service.ReviewService;

public class ReviewService : IReviewService
{
    private readonly IReviewRepository reviewRepository;
    private readonly ILogger<ReviewService> logger;
    private readonly IMapper mapper;

    public ReviewService(
        IReviewRepository reviewRepository,
        ILogger<ReviewService> logger,
        IMapper mapper)
    {
        this.reviewRepository = reviewRepository;
        this.logger = logger;
        this.mapper = mapper;
    }

    public async Task<ReviewDto> CreateAsync(ReviewDto reviewDto)
    {
        try
        {
            (reviewDto.CreatedOn, reviewDto.ModifiedOn) = (DateTime.Now, DateTime.Now);

            logger.LogInformation($"Mapping ReviewDto to Review (Entity) in ReviewService CreateAsync");

            var review = mapper.Map<Review>(reviewDto);

            return await reviewRepository.CreateAsync(review, reviewDto);
        }
        catch (Exception e)
        {

            logger.LogError(e, $"Failed to map Review. Error: {e.Message}.");

            throw new ApplicationException(e.Message);
        }
    }

    public async Task<bool> DeleteByIdAsync(int id)
        => await reviewRepository.DeleteByIdAsync(id);

    public async Task<ReviewDto[]> GetAllAsync()
        => await reviewRepository.GetAllAsync();

    public async Task<ReviewDto> GetByIdAsync(int id)
        => await reviewRepository.GetByIdAsync(id);

    public async Task<ReviewDto> UpdateAsync(int id, ReviewDto reviewDto)
    {
        try
        {
            logger.LogInformation($"Mapping ReviewDto to Review (Entity) in ReviewService UpdateAsync");
            var review = mapper.Map<Review>(reviewDto);

            return await reviewRepository.UpdateAsync(id, review);
        }
        catch (Exception e)
        {
            logger.LogError(e, $"Failed to map Review. Error: {e.Message}.");

            throw new ApplicationException(e.Message);
        }
    }
}