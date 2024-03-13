using FilmForge.Repository.RatingRepository;

namespace FilmForge.Service.RatingService;

public class RatingService : IRatingService
{
    private readonly IRatingRepository ratingRepository;
    private readonly ILogger<RatingService> logger;
    private readonly IMapper mapper;

    public RatingService(
        IRatingRepository ratingRepository,
        ILogger<RatingService> logger,
        IMapper mapper)
    {
        this.ratingRepository = ratingRepository;
        this.logger = logger;
        this.mapper = mapper;
    }

    public async Task<RatingDto> CreateAsync(RatingDto ratingDto)
    {
        try
        {
            (ratingDto.CreatedOn, ratingDto.ModifiedOn) = (DateTime.Now, DateTime.Now);

            logger.LogInformation($"Mapping RatingDto to Rating (Entity) in RatingService CreateAsync");

            var rating = mapper.Map<Rating>(ratingDto);

            return await ratingRepository.CreateAsync(rating, ratingDto);
        }
        catch (Exception e)
        {

            logger.LogError(e, $"Failed to map Rating. Error: {e.Message}.");

            throw new ApplicationException(e.Message);
        }
    }

    public async Task<bool> DeleteByIdAsync(int id)
        => await ratingRepository.DeleteByIdAsync(id);

    public async Task<RatingDto[]> GetAllAsync()
        => await ratingRepository.GetAllAsync();

    public async Task<RatingDto> GetByIdAsync(int id)
        => await ratingRepository.GetByIdAsync(id);

    public async Task<RatingDto> UpdateAsync(int id, RatingDto ratingDto)
    {
        try
        {
            logger.LogInformation($"Mapping RatingDto to Rating (Entity) in RatingService UpdateAsync");
            var rating = mapper.Map<Rating>(ratingDto);

            return await ratingRepository.UpdateAsync(id, rating);
        }
        catch (Exception e)
        {
            logger.LogError(e, $"Failed to map Rating. Error: {e.Message}.");

            throw new ApplicationException(e.Message);
        }
    }
}