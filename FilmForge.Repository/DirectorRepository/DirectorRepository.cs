namespace FilmForge.Repository.DirectorRepository;

public class DirectorRepository : IDirectorRepository
{
    private readonly FilmForgeDbContext dbContext;
    private readonly ILogger<DirectorRepository> logger;
    private readonly IMapper mapper;

    public DirectorRepository(
        FilmForgeDbContext dbContext,
        ILogger<DirectorRepository> logger,
        IMapper mapper)
    {
        this.dbContext = dbContext;
        this.logger = logger;
        this.mapper = mapper;
    }

    public async Task<DirectorDto> CreateAsync(Director director, DirectorDto directorDto)
    {
        logger.LogInformation("Adding a new Director.");
        try
        {
            await dbContext.AddAsync(director);
            await dbContext.SaveChangesAsync();

            logger.LogInformation($"Director created successfully with Name: {director.Name}.");

            return mapper.Map<DirectorDto>(director);
        }
        catch (Exception e)
        {
            logger.LogError(e, $"Failed to add Director. Error: {e.Message}.");

            throw new ApplicationException(e.Message);
        }
    }

    public async Task<bool> DeleteByIdAsync(int id)
    {
        logger.LogInformation($"Atempting to delete Director by id: {id}.");

        try
        {
            var director = await dbContext
                .Directors
                .FindAsync(id);

            if (director == null)
            {
                logger.LogWarning($"Director with ID: {id} not found.");

                return false;
            }

            dbContext.Directors.Remove(director);
            await dbContext.SaveChangesAsync();

            logger.LogInformation($"Director with id: {id} successfully deleted.");

            return true;

        }
        catch (Exception e)
        {
            logger.LogError(e, $"Failed to delete Director with id: {id}. Error: {e.Message}.");

            throw new ApplicationException(e.Message);
        }
    }

    public async Task<DirectorDto[]> GetAllAsync()
    {
        logger.LogInformation("Getting all Directors.");

        try
        {
            var directors = await dbContext
                .Directors
                .ToArrayAsync();

            if (directors == null)
            {
                logger.LogWarning("No Directors not found.");

                return null;
            }

            var directorDtos = mapper.Map<DirectorDto[]>(directors);

            logger.LogInformation($"Retrieved all {directorDtos.Count()} Directors.");

            return directorDtos;
        }
        catch (Exception e)
        {
            logger.LogError(e, $"Failed to retrieved all Director. Error: {e.Message}.");

            throw new ApplicationException(e.Message);
        }
    }

    public async Task<DirectorDto> GetByIdAsync(int id)
    {
        logger.LogInformation($"Getting Director with id: {id}.");

        try
        {
            var director = await dbContext
                .Directors
                .FindAsync(id);

            if (director == null)
            {
                logger.LogWarning($"Director with id: {id} not found.");

                return null;
            }

            logger.LogInformation($"Director found successfully with Id: {director.Id} and Name: {director.Name}.");

            return mapper.Map<DirectorDto>(director);
        }
        catch (Exception e)
        {
            logger.LogError(e, $"Failed to find Director with id {id}. Error: {e.Message}.");

            throw new ApplicationException(e.Message);
        }
    }

    public async Task<DirectorDto> UpdateAsync(int id, Director director)
    {
        logger.LogInformation($"Updating Director: {director.Name}.");

        try
        {
            var directorEntity = await dbContext
                .Directors
                .FindAsync(director.Id);

            if (director == null)
            {
                logger.LogWarning($"Director with id: {id} not found.");

                return null;
            }

            director.CreatedOn = directorEntity.CreatedOn;
            director.ModifiedOn = DateTime.Now;

            directorEntity = mapper.Map<Director>(director);

            dbContext.Entry(directorEntity).State = EntityState.Modified;
            await dbContext.SaveChangesAsync();

            logger.LogInformation($"Director updated successfully with Name: {director.Name}.");

            return mapper.Map<DirectorDto>(directorEntity);
        }
        catch (Exception e)
        {
            logger.LogError(e, $"Failed to update Director. Error: {e.Message}.");

            throw new ApplicationException(e.Message);
        }
    }
}