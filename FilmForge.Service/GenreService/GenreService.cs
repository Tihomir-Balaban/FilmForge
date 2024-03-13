using FilmForge.Repository.GenreRepository;

namespace FilmForge.Service.GenreService;

public class GenreService : IGenreService
{
    private readonly IGenreRepository genreRepository;
    private readonly ILogger<GenreService> logger;
    private readonly IMapper mapper;

    public GenreService(
        IGenreRepository genreRepository,
        ILogger<GenreService> logger,
        IMapper mapper)
    {
        this.genreRepository = genreRepository;
        this.logger = logger;
        this.mapper = mapper;
    }

    public async Task<GenreDto> CreateAsync(GenreDto genreDto)
    {
        try
        {
            (genreDto.CreatedOn, genreDto.ModifiedOn) = (DateTime.Now, DateTime.Now);

            logger.LogInformation($"Mapping GenreDto to Genre (Entity) in GenreService CreateAsync");

            var genre = mapper.Map<Genre>(genreDto);

            return await genreRepository.CreateAsync(genre, genreDto);
        }
        catch (Exception e)
        {

            logger.LogError(e, $"Failed to map Genre. Error: {e.Message}.");

            throw new ApplicationException(e.Message);
        }
    }

    public async Task<bool> DeleteByIdAsync(int id)
        => await genreRepository.DeleteByIdAsync(id);

    public async Task<GenreDto[]> GetAllAsync()
        => await genreRepository.GetAllAsync();

    public async Task<GenreDto> GetByIdAsync(int id)
        => await genreRepository.GetByIdAsync(id);

    public async Task<GenreDto> UpdateAsync(int id, GenreDto genreDto)
    {
        try
        {
            logger.LogInformation($"Mapping GenreDto to Genre (Entity) in GenreService UpdateAsync");
            var genre = mapper.Map<Genre>(genreDto);

            return await genreRepository.UpdateAsync(id, genre);
        }
        catch (Exception e)
        {
            logger.LogError(e, $"Failed to map Genre. Error: {e.Message}.");

            throw new ApplicationException(e.Message);
        }
    }
}