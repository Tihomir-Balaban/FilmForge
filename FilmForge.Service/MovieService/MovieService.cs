using FilmForge.Repository.MovieRepository;

namespace FilmForge.Service.MovieService;

public class MovieService : IMovieService
{
    private readonly IMovieRepository movieRepository;
    private readonly ILogger<MovieService> logger;
    private readonly IMapper mapper;

    public MovieService(
        IMovieRepository movieRepository,
        ILogger<MovieService> logger,
        IMapper mapper)
    {
        this.movieRepository = movieRepository;
        this.logger = logger;
        this.mapper = mapper;
    }

    public async Task<MovieDto> CreateAsync(MovieDto movieDto)
    {
        try
        {
            (movieDto.CreatedOn, movieDto.ModifiedOn) = (DateTime.Now, DateTime.Now);

            logger.LogInformation($"Mapping MovieDto to Movie (Entity) in MovieService CreateAsync");

            var movie = mapper.Map<Movie>(movieDto);

            return await movieRepository.CreateAsync(movie, movieDto);
        }
        catch (Exception e)
        {

            logger.LogError(e, $"Failed to map Movie. Error: {e.Message}.");

            throw new ApplicationException(e.Message);
        }
    }

    public async Task<bool> DeleteByIdAsync(int id)
        => await movieRepository.DeleteByIdAsync(id);

    public async Task<MovieDto[]> GetAllAsync()
        => await movieRepository.GetAllAsync();

    public async Task<MovieDto[]> GetByDirectorIdAsync(int directorId)
        => await movieRepository.GetByDirectorIdAsync(directorId);

    public async Task<MovieDto> GetByIdAsync(int id)
        => await movieRepository.GetByIdAsync(id);

    public async Task<MovieDto> UpdateAsync(int id, MovieDto movieDto)
    {
        try
        {
            logger.LogInformation($"Mapping MovieDto to Movie (Entity) in MovieService UpdateAsync");
            var movie = mapper.Map<Movie>(movieDto);

            return await movieRepository.UpdateAsync(id, movie);
        }
        catch (Exception e)
        {
            logger.LogError(e, $"Failed to map Movie. Error: {e.Message}.");

            throw new ApplicationException(e.Message);
        }
    }
}