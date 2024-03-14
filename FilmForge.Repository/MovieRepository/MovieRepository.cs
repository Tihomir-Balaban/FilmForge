using FilmForge.Models.Dtos;
using FilmForge.Models.Statics;
using FilmForge.Repository.ActorRepository;

namespace FilmForge.Repository.MovieRepository;

public class MovieRepository : IMovieRepository
{
    private readonly FilmForgeDbContext dbContext;
    private readonly ILogger<MovieRepository> logger;
    private readonly IMapper mapper;

    public MovieRepository(
        FilmForgeDbContext dbContext,
        ILogger<MovieRepository> logger,
        IMapper mapper)
    {
        this.dbContext = dbContext;
        this.logger = logger;
        this.mapper = mapper;
    }

    public async Task<MovieDto> CreateAsync(Movie movie, MovieDto movieDto)
    {
        logger.LogInformation("Adding a new Movie.");
        try
        {
            await dbContext.AddAsync(movie);
            await dbContext.SaveChangesAsync();

            logger.LogInformation($"Movie created successfully with Name: {movie.Title}.");

            return mapper.Map<MovieDto>(movie);
        }
        catch (Exception e)
        {
            logger.LogError(e, $"Failed to add Movie. Error: {e.Message}.");

            throw new ApplicationException(e.Message);
        }
    }

    public async Task<bool> DeleteByIdAsync(int id)
    {
        logger.LogInformation($"Atempting to delete Movie by id: {id}.");

        try
        {
            var movie = await dbContext
                .Movies
                .FindAsync(id);

            if (movie == null)
            {
                logger.LogWarning($"Movie with ID: {id} not found.");

                return false;
            }

            dbContext.Movies.Remove(movie);
            await dbContext.SaveChangesAsync();

            logger.LogInformation($"Movie with id: {id} successfully deleted.");

            return true;

        }
        catch (Exception e)
        {
            logger.LogError(e, $"Failed to delete Movie with id: {id}. Error: {e.Message}.");

            throw new ApplicationException(e.Message);
        }
    }

    public async Task<MovieDto[]> GetAllAsync()
    {
        logger.LogInformation("Getting all Movies.");

        try
        {
            var movies = await dbContext
                .Movies
                .ToArrayAsync();

            if (movies == null)
            {
                logger.LogWarning("No Movies not found.");

                return null;
            }

            var movieDtos = mapper.Map<MovieDto[]>(movies);

            logger.LogInformation($"Retrieved all {movieDtos.Count()} Movies.");

            return movieDtos;
        }
        catch (Exception e)
        {
            logger.LogError(e, $"Failed to retrieved all Movie. Error: {e.Message}.");

            throw new ApplicationException(e.Message);
        }
    }

    public async Task<MovieDto[]> GetByDirectorIdAsync(int directorId)
    {
        logger.LogInformation($"Getting Movie('s) with director id: {directorId}.");

        try
        {
            var movies = await dbContext
                .Movies
                .Where(m => m.DirectorId == directorId)
                .ToArrayAsync();

            if (movies == null)
            {
                logger.LogWarning($"Movie('s) with director id: {directorId} not found.");

                return null;
            }

            logger.LogInformation($"{movies.Count()} Movie('s) found successfully.");

            var movieDtos = mapper.Map<MovieDto[]>(movies);

            GetActorsForMovies(movieDtos);

            return movieDtos;
        }
        catch (Exception e)
        {
            logger.LogError(e, $"Failed to find Movie('s) with director id: {directorId}. Error: {e.Message}.");

            throw new ApplicationException(e.Message);
        }
    }

    public async Task<MovieDto> GetByIdAsync(int id)
    {
        logger.LogInformation($"Getting Movie with id: {id}.");

        try
        {
            var movie = await dbContext
                .Movies
                .FindAsync(id);

            if (movie == null)
            {
                logger.LogWarning($"Movie with id: {id} not found.");

                return null;
            }

            logger.LogInformation($"Movie found successfully with Id: {movie.Id} and Name: {movie.Title}.");

            var movieDto = mapper.Map<MovieDto>(movie);

            GetActorsForMovies(movieDto);

            return movieDto;
        }
        catch (Exception e)
        {
            logger.LogError(e, $"Failed to find Movie with id {id}. Error: {e.Message}.");

            throw new ApplicationException(e.Message);
        }
    }

    public async Task<MovieDto> UpdateAsync(int id, Movie movie)
    {
        logger.LogInformation($"Updating Movie: {movie.Title}.");

        try
        {
            var movieEntity = await dbContext
                .Movies
                .FindAsync(movie.Id);

            if (movie == null)
            {
                logger.LogWarning($"Movie with id: {id} not found.");

                return null;
            }

            movie.CreatedOn = movieEntity.CreatedOn;
            movie.ModifiedOn = DateTime.Now;

            movieEntity = mapper.Map<Movie>(movie);

            dbContext.Entry(movieEntity).State = EntityState.Modified;
            await dbContext.SaveChangesAsync();

            logger.LogInformation($"Movie updated successfully with Name: {movie.Title}.");

            return mapper.Map<MovieDto>(movieEntity);
        }
        catch (Exception e)
        {
            logger.LogError(e, $"Failed to update Movie. Error: {e.Message}.");

            throw new ApplicationException(e.Message);
        }
    }

    public async Task<MovieDto> GetMovieByActorIdAsync(int id)
    {
        try
        {
            logger.LogInformation($"Getting movies of this actor by id {id}");

            var movie = await dbContext
                .Actors
                .Where(m => m.Id == id)
                .SelectMany(m => m.Movies)
                .FirstOrDefaultAsync();

            if (DateTime.Now.IsDateBetween(movie.StartDate, movie.ReleaseDate))
            {
                logger.LogWarning($"Movie {movie.Title} is in Production and Actor is locked in");

                throw new ApplicationException($"Movie {movie.Title} is in Production and Actor is locked in");
            }

            return mapper.Map<MovieDto>(movie);
        }
        catch (Exception e)
        {
            logger.LogError(e, $"Failed to find Movie with Actor id {id}. Error: {e.Message}.");

            throw new ApplicationException(e.Message);
        }
    }

    private void GetActorsForMovies(params MovieDto[] movieDtos)
    {
        logger.LogInformation($"Getting actors of these movies");

        foreach (var movieDto in movieDtos)
        {
            var actors = dbContext
                .Movies
                .Where(m => m.Id == movieDto.Id)
                .SelectMany(m => m.Actors)
                .ToArray();

            movieDto.Actors = mapper.Map<ActorDto[]>(actors);
        }
    }
}