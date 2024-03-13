using FilmForge.Repository.ActorRepository;

namespace FilmForge.Service.ActorService;

public class ActorService : IActorService
{
    private readonly IActorRepository actorRepository;
    private readonly ILogger<ActorService> logger;
    private readonly IMapper mapper;

    public ActorService(
        IActorRepository actorRepository,
        ILogger<ActorService> logger,
        IMapper mapper)
    {
        this.actorRepository = actorRepository;
        this.logger = logger;
        this.mapper = mapper;
    }

    public async Task<ActorDto> CreateAsync(ActorDto actorDto)
    {
        try
        {
            (actorDto.CreatedOn, actorDto.ModifiedOn) = (DateTime.Now, DateTime.Now);

            logger.LogInformation($"Mapping ActorDto to Actor (Entity) in UserService CreateAsync");

            var actor = mapper.Map<Actor>(actorDto);

            return await actorRepository.CreateAsync(actor, actorDto);
        }
        catch (Exception e)
        {

            logger.LogError(e, $"Failed to map Actor. Error: {e.Message}.");

            throw new ApplicationException(e.Message);
        }
    }

    public async Task<bool> DeleteByIdAsync(int id)
        => await actorRepository.DeleteByIdAsync(id);

    public async Task<ActorDto[]> GetAllAsync()
        => await actorRepository.GetAllAsync();

    public async Task<ActorDto> GetByIdAsync(int id)
        => await actorRepository.GetByIdAsync(id);

    public async Task<ActorDto> UpdateAsync(int id, ActorDto actorDto)
    {
        try
        {
            logger.LogInformation($"Mapping ActorDto to Actor (Entity) in UserService UpdateAsync");
            var actor = mapper.Map<Actor>(actorDto);

            return await actorRepository.UpdateAsync(id, actor);
        }
        catch (Exception e)
        {
            logger.LogError(e, $"Failed to map Actor. Error: {e.Message}.");

            throw new ApplicationException(e.Message);
        }
    }
}