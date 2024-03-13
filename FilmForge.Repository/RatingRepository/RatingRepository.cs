namespace FilmForge.Repository.RatingRepository;

public class RatingRepository : IRatingRepository
{
    private readonly FilmForgeDbContext dbContext;
    private readonly ILogger<RatingRepository> logger;
    private readonly IMapper mapper;

    public RatingRepository(
        FilmForgeDbContext dbContext,
        ILogger<RatingRepository> logger,
        IMapper mapper)
    {
        this.dbContext = dbContext;
        this.logger = logger;
        this.mapper = mapper;
    }

    public async Task<RatingDto> CreateAsync(Rating rating, RatingDto ratingDto)
    {
        logger.LogInformation("Adding a new Rating.");
        try
        {
            await dbContext.AddAsync(rating);
            await dbContext.SaveChangesAsync();

            logger.LogInformation($"Rating created successfully with Name: {rating.Name}.");

            return mapper.Map<RatingDto>(rating);
        }
        catch (Exception e)
        {
            logger.LogError(e, $"Failed to add Rating. Error: {e.Message}.");

            throw new ApplicationException(e.Message);
        }
    }

    public async Task<bool> DeleteByIdAsync(int id)
    {
        logger.LogInformation($"Atempting to delete Rating by id: {id}.");

        try
        {
            var rating = await dbContext
                .Ratings
                .FindAsync(id);

            if (rating == null)
            {
                logger.LogWarning($"Rating with ID: {id} not found.");

                return false;
            }

            dbContext.Ratings.Remove(rating);
            await dbContext.SaveChangesAsync();

            logger.LogInformation($"Rating with id: {id} successfully deleted.");

            return true;

        }
        catch (Exception e)
        {
            logger.LogError(e, $"Failed to delete Rating with id: {id}. Error: {e.Message}.");

            throw new ApplicationException(e.Message);
        }
    }

    public async Task<RatingDto[]> GetAllAsync()
    {
        logger.LogInformation("Getting all Ratings.");

        try
        {
            var ratings = await dbContext
                .Ratings
                .ToArrayAsync();

            if (ratings == null)
            {
                logger.LogWarning("No Ratings not found.");

                return null;
            }

            var ratingDtos = mapper.Map<RatingDto[]>(ratings);

            logger.LogInformation($"Retrieved all {ratingDtos.Count()} Ratings.");

            return ratingDtos;
        }
        catch (Exception e)
        {
            logger.LogError(e, $"Failed to retrieved all Rating. Error: {e.Message}.");

            throw new ApplicationException(e.Message);
        }
    }

    public async Task<RatingDto> GetByIdAsync(int id)
    {
        logger.LogInformation($"Getting Rating with id: {id}.");

        try
        {
            var rating = await dbContext
                .Ratings
                .FindAsync(id);

            if (rating == null)
            {
                logger.LogWarning($"Rating with id: {id} not found.");

                return null;
            }

            logger.LogInformation($"Rating found successfully with Id: {rating.Id} and Name: {rating.Name}.");

            return mapper.Map<RatingDto>(rating);
        }
        catch (Exception e)
        {
            logger.LogError(e, $"Failed to find Rating with id {id}. Error: {e.Message}.");

            throw new ApplicationException(e.Message);
        }
    }

    public async Task<RatingDto> UpdateAsync(int id, Rating rating)
    {
        logger.LogInformation($"Updating Rating: {rating.Name}.");

        try
        {
            var ratingEntity = await dbContext
                .Ratings
                .FindAsync(rating.Id);

            if (rating == null)
            {
                logger.LogWarning($"Rating with id: {id} not found.");

                return null;
            }

            rating.CreatedOn = ratingEntity.CreatedOn;
            rating.ModifiedOn = DateTime.Now;

            ratingEntity = mapper.Map<Rating>(rating);

            dbContext.Entry(ratingEntity).State = EntityState.Modified;
            await dbContext.SaveChangesAsync();

            logger.LogInformation($"Rating updated successfully with Name: {rating.Name}.");

            return mapper.Map<RatingDto>(ratingEntity);
        }
        catch (Exception e)
        {
            logger.LogError(e, $"Failed to update Rating. Error: {e.Message}.");

            throw new ApplicationException(e.Message);
        }
    }
}