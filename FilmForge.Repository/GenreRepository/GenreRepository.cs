namespace FilmForge.Repository.GenreRepository;

public class GenreRepository : IGenreRepository
{
    private readonly FilmForgeDbContext dbContext;
    private readonly ILogger<GenreRepository> logger;
    private readonly IMapper mapper;

    public GenreRepository(
        FilmForgeDbContext dbContext,
        ILogger<GenreRepository> logger,
        IMapper mapper)
    {
        this.dbContext = dbContext;
        this.logger = logger;
        this.mapper = mapper;
    }

    public async Task<GenreDto> CreateAsync(Genre genre, GenreDto genreDto)
    {
        logger.LogInformation("Adding a new Genre.");
        try
        {
            await dbContext.AddAsync(genre);
            await dbContext.SaveChangesAsync();

            logger.LogInformation($"Genre created successfully with Name: {genre.Name}.");

            return mapper.Map<GenreDto>(genre);
        }
        catch (Exception e)
        {
            logger.LogError(e, $"Failed to add Genre. Error: {e.Message}.");

            throw new ApplicationException(e.Message);
        }
    }

    public async Task<bool> DeleteByIdAsync(int id)
    {
        logger.LogInformation($"Atempting to delete Genre by id: {id}.");

        try
        {
            var genre = await dbContext
                .Genres
                .FindAsync(id);

            if (genre == null)
            {
                logger.LogWarning($"Genre with ID: {id} not found.");

                return false;
            }

            dbContext.Genres.Remove(genre);
            await dbContext.SaveChangesAsync();

            logger.LogInformation($"Genre with id: {id} successfully deleted.");

            return true;

        }
        catch (Exception e)
        {
            logger.LogError(e, $"Failed to delete Genre with id: {id}. Error: {e.Message}.");

            throw new ApplicationException(e.Message);
        }
    }

    public async Task<GenreDto[]> GetAllAsync()
    {
        logger.LogInformation("Getting all Genres.");

        try
        {
            var genres = await dbContext
                .Genres
                .ToArrayAsync();

            if (genres == null)
            {
                logger.LogWarning("No Genres not found.");

                return null;
            }

            var genreDtos = mapper.Map<GenreDto[]>(genres);

            logger.LogInformation($"Retrieved all {genreDtos.Count()} Genres.");

            return genreDtos;
        }
        catch (Exception e)
        {
            logger.LogError(e, $"Failed to retrieved all Genre. Error: {e.Message}.");

            throw new ApplicationException(e.Message);
        }
    }

    public async Task<GenreDto> GetByIdAsync(int id)
    {
        logger.LogInformation($"Getting Genre with id: {id}.");

        try
        {
            var genre = await dbContext
                .Genres
                .FindAsync(id);

            if (genre == null)
            {
                logger.LogWarning($"Genre with id: {id} not found.");

                return null;
            }

            logger.LogInformation($"Genre found successfully with Id: {genre.Id} and Name: {genre.Name}.");

            return mapper.Map<GenreDto>(genre);
        }
        catch (Exception e)
        {
            logger.LogError(e, $"Failed to find Genre with id {id}. Error: {e.Message}.");

            throw new ApplicationException(e.Message);
        }
    }

    public async Task<GenreDto> UpdateAsync(int id, Genre genre)
    {
        logger.LogInformation($"Updating Genre: {genre.Name}.");

        try
        {
            var genreEntity = await dbContext
                .Genres
                .FindAsync(genre.Id);

            if (genre == null)
            {
                logger.LogWarning($"Genre with id: {id} not found.");

                return null;
            }

            genre.CreatedOn = genreEntity.CreatedOn;
            genre.ModifiedOn = DateTime.Now;

            genreEntity = mapper.Map<Genre>(genre);

            dbContext.Entry(genreEntity).State = EntityState.Modified;
            await dbContext.SaveChangesAsync();

            logger.LogInformation($"Genre updated successfully with Name: {genre.Name}.");

            return mapper.Map<GenreDto>(genreEntity);
        }
        catch (Exception e)
        {
            logger.LogError(e, $"Failed to update Genre. Error: {e.Message}.");

            throw new ApplicationException(e.Message);
        }
    }
}