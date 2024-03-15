using FilmForge.Models.Dtos;

namespace FilmForge.Repository.ActorRepository;

public class ActorRepository : IActorRepository
{
    private readonly FilmForgeDbContext dbContext;
    private readonly ILogger<ActorRepository> logger;
    private readonly IMapper mapper;

    public ActorRepository(
        FilmForgeDbContext dbContext,
        ILogger<ActorRepository> logger,
        IMapper mapper)
    {
        this.dbContext = dbContext;
        this.logger = logger;
        this.mapper = mapper;
    }

    public async Task<ActorDto> CreateAsync(Actor actor, ActorDto actorDto)
    {
        logger.LogInformation("Adding a new Actor.");
        try
        {
            await dbContext.AddAsync(actor);
            await dbContext.SaveChangesAsync();

            logger.LogInformation($"Actor created successfully with Name: {actor.Name}.");

            return mapper.Map<ActorDto>(actor);
        }
        catch (Exception e)
        {
            logger.LogError(e, $"Failed to add Actor. Error: {e.Message}.");

            throw new ApplicationException(e.Message);
        }
    }

    public async Task<bool> DeleteByIdAsync(int id)
    {
        logger.LogInformation($"Atempting to delete Actor by id: {id}.");

        try
        {
            var actor = await dbContext
                .Actors
                .FindAsync(id);

            if (actor == null)
            {
                logger.LogWarning($"Actor with ID: {id} not found.");

                return false;
            }

            dbContext.Actors.Remove(actor);
            await dbContext.SaveChangesAsync();

            logger.LogInformation($"Actor with id: {id} successfully deleted.");

            return true;

        }
        catch (Exception e)
        {
            logger.LogError(e, $"Failed to delete Actor with id: {id}. Error: {e.Message}.");

            throw new ApplicationException(e.Message);
        }
    }

    public async Task<ActorDto[]> GetAllAsync()
    {
        logger.LogInformation("Getting all Actors.");

        try
        {
            var actors = await dbContext
                .Actors
                .ToArrayAsync();

            if (actors == null)
            {
                logger.LogWarning("No Actors not found.");

                return null;
            }

            var actorDtos = mapper.Map<ActorDto[]>(actors);

            logger.LogInformation($"Retrieved all {actorDtos.Count()} Actors.");

            return actorDtos;
        }
        catch (Exception e)
        {
            logger.LogError(e, $"Failed to retrieved all Actor. Error: {e.Message}.");

            throw new ApplicationException(e.Message);
        }
    }

    public async Task<ActorDto> GetByIdAsync(int id)
    {
        logger.LogInformation($"Getting Actor with id: {id}.");

        try
        {
            var actor = await dbContext
                .Actors
                .FindAsync(id);

            if (actor == null)
            {
                logger.LogWarning($"Actor with id: {id} not found.");

                return null;
            }

            logger.LogInformation($"Actor found successfully with Id: {actor.Id} and Name: {actor.Name}.");

            return mapper.Map<ActorDto>(actor);
        }
        catch (Exception e)
        {
            logger.LogError(e, $"Failed to find Actor with id {id}. Error: {e.Message}.");

            throw new ApplicationException(e.Message);
        }
    }

    public async Task<ActorDto> UpdateAsync(int id, Actor actor)
    {
        logger.LogInformation($"Updating Actor: {actor.Name}.");

        try
        {
            var actorEntity = await dbContext
                .Actors
                .FindAsync(actor.Id);

            if (actor == null)
            {
                logger.LogWarning($"Actor with id: {id} not found.");

                return null;
            }

            actor.CreatedOn = actorEntity.CreatedOn;
            actor.ModifiedOn = DateTime.Now;

            actorEntity = mapper.Map<Actor>(actor);

            await dbContext.SaveChangesAsync();

            logger.LogInformation($"Actor updated successfully with Name: {actor.Name}.");

            return mapper.Map<ActorDto>(actorEntity);
        }
        catch (Exception e)
        {
            logger.LogError(e, $"Failed to update Actor. Error: {e.Message}.");

            throw new ApplicationException(e.Message);
        }
    }
}