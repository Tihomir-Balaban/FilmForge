namespace FilmForge.Repository.InvitationRepository;

public class InvitationRepository : IInvitationRepository
{
    private readonly FilmForgeDbContext dbContext;
    private readonly ILogger<InvitationRepository> logger;
    private readonly IMapper mapper;

    public InvitationRepository(
        FilmForgeDbContext dbContext,
        ILogger<InvitationRepository> logger,
        IMapper mapper)
    {
        this.dbContext = dbContext;
        this.logger = logger;
        this.mapper = mapper;
    }

    public async Task<InvitationDto> CreateAsync(Invitation invitation)

    {
        logger.LogInformation("Adding a new Invitation.");

        try
        {
            await dbContext.AddAsync(invitation);
            await dbContext.SaveChangesAsync();

            logger.LogInformation($"Invitation created successfully with Actor id: {invitation.ActorId} and Movie id: {invitation.MovieId}.");

            return mapper.Map<InvitationDto>(invitation);
        }
        catch (Exception e)
        {
            logger.LogError(e, $"Failed to add Actor. Error: {e.Message}.");

            throw new ApplicationException(e.Message);
        }
    }

    public async Task<bool> DeleteByIdAsync(int id)
    {
        logger.LogInformation($"Atempting to delete Invitation by id: {id}.");

        try
        {
            var invitation = await dbContext
                .Invitations
                .FindAsync(id);

            if (invitation == null)
            {
                logger.LogWarning($"Invitation with ID: {id} not found.");

                return false;
            }

            dbContext.Invitations.Remove(invitation);
            await dbContext.SaveChangesAsync();

            logger.LogInformation($"Invitation with id: {id} successfully deleted.");

            return true;

        }
        catch (Exception e)
        {
            logger.LogError(e, $"Failed to delete Invitation with id: {id}. Error: {e.Message}.");

            throw new ApplicationException(e.Message);
        }
    }

    public async Task<InvitationDto> GetByActorIdAndMovieIdAsync(int actorId, int movieId)
    {
        logger.LogInformation($"Getting Invitation with Movie id: {movieId} and Actor id: {actorId}.");

        try
        {
            var invitation = await dbContext
            .Invitations
                .Where(i => 
                    i.MovieId.Equals(movieId) &&
                    i.ActorId.Equals(actorId))
                .FirstOrDefaultAsync();

            if (invitation == null)
            {
                logger.LogWarning($"Invitation with Movie id: {movieId} and Actor id: {actorId}.");

                return null;
            }

            logger.LogInformation($"Invitation found successfully with Movie id: {movieId} and Actor id: {actorId}.");

            return mapper.Map<InvitationDto>(invitation);
        }
        catch (Exception e)
        {
            logger.LogError(e, $"Failed to find Invitation with Movie id: {movieId} and Actor id: {actorId}. Error: {e.Message}.");

            throw new ApplicationException(e.Message);
        }
    }

    public async Task<InvitationDto[]> GetByActorIdAsync(int actorId)
    {
        logger.LogInformation($"Getting Invitation('s) with Actor id: {actorId}.");

        try
        {
            var invitations = await dbContext
                .Invitations
                .Where(i => i.ActorId.Equals(actorId))
                .ToArrayAsync();

            if (invitations == null)
            {
                logger.LogWarning($"Invitation('s) with Actor id: {actorId} not found.");

                return null;
            }

            logger.LogInformation($"{invitations.Count()} Invitation('s) found successfully.");

            return mapper.Map<InvitationDto[]>(invitations);
        }
        catch (Exception e)
        {
            logger.LogError(e, $"Failed to find Invitation('s) with Actor id {actorId}. Error: {e.Message}.");

            throw new ApplicationException(e.Message);
        }
    }

    public async Task<InvitationDto[]> GetByMovieIdAsync(int movieId)
    {
        logger.LogInformation($"Getting Invitation('s) with Movie id: {movieId}.");

        try
        {
            var invitations = await dbContext
            .Invitations
                .Where(i => i.MovieId.Equals(movieId))
                .ToArrayAsync();

            if (invitations == null)
            {
                logger.LogWarning($"Invitation('s) with Movie id: {movieId} not found.");

                return null;
            }

            logger.LogInformation($"{invitations.Count()} Invitation('s) found successfully.");

            return mapper.Map<InvitationDto[]>(invitations);
        }
        catch (Exception e)
        {
            logger.LogError(e, $"Failed to find Invitation('s) with Movie id {movieId}. Error: {e.Message}.");

            throw new ApplicationException(e.Message);
        }
    }

    public async Task<InvitationDto> UpdateAsync(int id, Invitation invitation)
    {
        logger.LogInformation($"Updating Invitation: {id}.");

        try
        {
            var invitationEntity = await dbContext
                .Invitations
                .FindAsync(id);

            if (invitation == null)
            {
                logger.LogWarning($"Invitation with id: {id} not found.");

                return null;
            }

            invitation.CreatedOn = invitationEntity.CreatedOn;
            invitation.ModifiedOn = DateTime.Now;

            invitationEntity = mapper.Map<Invitation>(invitation);

            dbContext.Entry(invitationEntity).State = EntityState.Modified;
            await dbContext.SaveChangesAsync();

            logger.LogInformation($"Invitation updated successfully with Id: {invitation.Id}.");

            return mapper.Map<InvitationDto>(invitationEntity);
        }
        catch (Exception e)
        {
            logger.LogError(e, $"Failed to update Invitation. Error: {e.Message}.");

            throw new ApplicationException(e.Message);
        }
    }
}