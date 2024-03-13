namespace FilmForge.Repository.ReviewRepository;

public class ReviewRepository : IReviewRepository
{
    private readonly FilmForgeDbContext dbContext;
    private readonly ILogger<ReviewRepository> logger;
    private readonly IMapper mapper;

    public ReviewRepository(
        FilmForgeDbContext dbContext,
        ILogger<ReviewRepository> logger,
        IMapper mapper)
    {
        this.dbContext = dbContext;
        this.logger = logger;
        this.mapper = mapper;
    }

    public async Task<ReviewDto> CreateAsync(Review review, ReviewDto reviewDto)
    {
        logger.LogInformation("Adding a new Review.");
        try
        {
            await dbContext.AddAsync(review);
            await dbContext.SaveChangesAsync();

            logger.LogInformation($"Review created successfully with MovieId: {review.MovieId}.");

            return mapper.Map<ReviewDto>(review);
        }
        catch (Exception e)
        {
            logger.LogError(e, $"Failed to add Review. Error: {e.Message}.");

            throw new ApplicationException(e.Message);
        }
    }

    public async Task<bool> DeleteByIdAsync(int id)
    {
        logger.LogInformation($"Atempting to delete Review by id: {id}.");

        try
        {
            var review = await dbContext
                .Reviews
                .FindAsync(id);

            if (review == null)
            {
                logger.LogWarning($"Review with ID: {id} not found.");

                return false;
            }

            dbContext.Reviews.Remove(review);
            await dbContext.SaveChangesAsync();

            logger.LogInformation($"Review with id: {id} successfully deleted.");

            return true;

        }
        catch (Exception e)
        {
            logger.LogError(e, $"Failed to delete Review with id: {id}. Error: {e.Message}.");

            throw new ApplicationException(e.Message);
        }
    }

    public async Task<ReviewDto[]> GetAllAsync()
    {
        logger.LogInformation("Getting all Reviews.");

        try
        {
            var reviews = await dbContext
                .Reviews
                .ToArrayAsync();

            if (reviews == null)
            {
                logger.LogWarning("No Reviews not found.");

                return null;
            }

            var reviewDtos = mapper.Map<ReviewDto[]>(reviews);

            logger.LogInformation($"Retrieved all {reviewDtos.Count()} Reviews.");

            return reviewDtos;
        }
        catch (Exception e)
        {
            logger.LogError(e, $"Failed to retrieved all Review. Error: {e.Message}.");

            throw new ApplicationException(e.Message);
        }
    }

    public async Task<ReviewDto> GetByIdAsync(int id)
    {
        logger.LogInformation($"Getting Review with id: {id}.");

        try
        {
            var review = await dbContext
                .Reviews
                .FindAsync(id);

            if (review == null)
            {
                logger.LogWarning($"Review with id: {id} not found.");

                return null;
            }

            logger.LogInformation($"Review found successfully with Id: {review.Id} and MovieId: {review.MovieId}.");

            return mapper.Map<ReviewDto>(review);
        }
        catch (Exception e)
        {
            logger.LogError(e, $"Failed to find Review with id {id}. Error: {e.Message}.");

            throw new ApplicationException(e.Message);
        }
    }

    public async Task<ReviewDto> UpdateAsync(int id, Review review)
    {
        logger.LogInformation($"Updating Review with MovieId: {review.MovieId}.");

        try
        {
            var reviewEntity = await dbContext
                .Reviews
                .FindAsync(review.Id);

            if (review == null)
            {
                logger.LogWarning($"Review with id: {id} not found.");

                return null;
            }

            review.CreatedOn = reviewEntity.CreatedOn;
            review.ModifiedOn = DateTime.Now;

            reviewEntity = mapper.Map<Review>(review);

            dbContext.Entry(reviewEntity).State = EntityState.Modified;
            await dbContext.SaveChangesAsync();

            logger.LogInformation($"Review updated successfully with MovieId: {review.MovieId}.");

            return mapper.Map<ReviewDto>(reviewEntity);
        }
        catch (Exception e)
        {
            logger.LogError(e, $"Failed to update Review. Error: {e.Message}.");

            throw new ApplicationException(e.Message);
        }
    }
}