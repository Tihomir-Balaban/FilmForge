using FilmForge.Repository.ActorRepository;
using FilmForge.Repository.MovieRepository;
using FilmForge.Models.Statics.ActorDto;
using Microsoft.EntityFrameworkCore;
using FilmForge.Models.Dtos;
using Microsoft.IdentityModel.Tokens;

namespace FilmForge.Service.ActorService;

public class ActorService : IActorService
{
    private readonly IActorRepository actorRepository;
    private readonly IMovieRepository movieRepository;
    private readonly ILogger<ActorService> logger;
    private readonly IMapper mapper;

    public ActorService(
        IActorRepository actorRepository,
        IMovieRepository movieRepository,
        ILogger<ActorService> logger,
        IMapper mapper)
    {
        this.actorRepository = actorRepository;
        this.movieRepository = movieRepository;
        this.logger = logger;
        this.mapper = mapper;
    }

    public async Task<ActorDto> CreateAsync(ActorDto actorDto)
    {
        try
        {
            (actorDto.CreatedOn, actorDto.ModifiedOn) = (DateTime.Now, DateTime.Now);

            logger.LogInformation($"Mapping ActorDto to Actor (Entity) in ActorService CreateAsync");

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
            var actorDbDto = await actorRepository.GetByIdAsync(id);

            if (actorDbDto != null && !actorDbDto.Fee.Equals(actorDto.Fee))
            {
                var movieDto = await movieRepository.GetMovieByActorIdAsync(id);

                UpdateActorFeeAndValidate(movieDto, actorDto.Id, actorDto.Fee);
            }

            logger.LogInformation($"Mapping ActorDto to Actor (Entity) in ActorService UpdateAsync");
            var actor = mapper.Map<Actor>(actorDto);

            return await actorRepository.UpdateAsync(id, actor);
        }
        catch (Exception e)
        {
            logger.LogError(e, $"Failed to map Actor. Error: {e.Message}.");

            throw new ApplicationException(e.Message);
        }
    }

    private void UpdateActorFeeAndValidate(MovieDto movieDto, int id, ulong fee)
    {
        var actorsDtos = new List<ActorDto>();

        var actorDto = movieDto
            .Actors
            .Where(a => a.Id.Equals(id))
            .FirstOrDefault();

        var actorDtosNotChanged = movieDto
            .Actors
            .Where(a => !a.Id.Equals(id));

        actorDto.Fee = fee;

        actorsDtos.Add(actorDto);
        actorsDtos.AddRange(actorDtosNotChanged);

        movieDto.Actors = actorsDtos;

        if (!movieDto.Actors.IsNullOrEmpty() && !movieDto.CheckMovieBugeting())
        {
            logger.LogWarning("Movie will be overbudget");

            throw new ApplicationException("Movie is overbudget");
        }
    }
}