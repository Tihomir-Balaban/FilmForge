using FilmForge.Models.Dtos;
using FilmForge.Repository.InvitationRepository;
using FilmForge.Service.ActorService;
using FilmForge.Service.MovieService;

namespace FilmForge.Service.InvitationService;

public class InvitationService : IInvitationService
{
    private readonly IInvitationRepository invitationRepository;
    private readonly IMovieService movieService;
    private readonly IActorService actorService;
    private readonly ILogger<InvitationService> logger;
    private readonly IMapper mapper;

    public InvitationService(
        IInvitationRepository invitationRepository,
        IMovieService movieService,
        IActorService actorService,
        ILogger<InvitationService> logger,
        IMapper mapper)
    {
        this.invitationRepository = invitationRepository;
        this.movieService = movieService;
        this.logger = logger;
        this.mapper = mapper;
    }

    public async Task<InvitationDto> CreateAsync(InvitationDto invitationDto)
    {
        try
        {
            if (!await ValidateBudgetAfterInviteAsync(invitationDto))
            {
                logger.LogWarning("Movie will be overbudget");

                throw new ApplicationException("Movie will be overbudget");
            }

            (invitationDto.CreatedOn, invitationDto.ModifiedOn) = (DateTime.Now, DateTime.Now);

            logger.LogInformation($"Mapping InvitationDto to Invitation (Entity) in InvitationService CreateAsync");

            var invitation = mapper.Map<Invitation>(invitationDto);

            return await invitationRepository.CreateAsync(invitation);
        }
        catch (Exception e)
        {
            logger.LogError(e, $"Failed to map Actor. Error: {e.Message}.");

            throw new ApplicationException(e.Message);
        }
    }

    public async Task<bool> DeleteByIdAsync(int id)
        => await invitationRepository.DeleteByIdAsync(id);

    public async Task<InvitationDto> GetByActorIdAndMovieIdAsync(int actorId, int movieId)
        => await invitationRepository.GetByActorIdAndMovieIdAsync(actorId, movieId);

    public async Task<InvitationDto[]> GetByActorIdAsync(int actorId)
        => await invitationRepository.GetByActorIdAsync(actorId);

    public async Task<InvitationDto[]> GetByMovieIdAsync(int movieId)
        => await invitationRepository.GetByMovieIdAsync(movieId);

    public async Task<InvitationDto> UpdateAsync(int id, InvitationDto invitationDto)
    {
        try
        {
            if (!await ValidateBudgetAfterInviteAsync(invitationDto))
            {
                logger.LogWarning("Movie will be overbudget");

                throw new ApplicationException("Movie will be overbudget");
            }

            logger.LogInformation($"Mapping InvitationDto to Invitation (Entity) in InvitationService UpdateAsync");
            var invitation = mapper.Map<Invitation>(invitationDto);

            return await invitationRepository.UpdateAsync(id, invitation);
        }
        catch (Exception e)
        {
            logger.LogError(e, $"Failed to map Invitation. Error: {e.Message}.");

            throw new ApplicationException(e.Message);
        };
    }

    private async Task<bool>  ValidateBudgetAfterInviteAsync(InvitationDto invitationDto)
    {
        var movieDto = await movieService.GetByIdAsync(invitationDto.MovieId);
        var actorDto = await actorService.GetByIdAsync(invitationDto.ActorId);

        movieDto.Actors.Add(actorDto);

        return movieDto.CheckMovieBugeting();
    }
}