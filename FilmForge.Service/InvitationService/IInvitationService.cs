namespace FilmForge.Service.InvitationService;

public interface IInvitationService
{
    Task<InvitationDto[]> GetByActorIdAsync(int actorId);
    Task<InvitationDto[]> GetByMovieIdAsync(int movieId);
    Task<InvitationDto> GetByActorIdAndMovieIdAsync(int actorId, int movieId);
    Task<InvitationDto> CreateAsync(InvitationDto invitationDto);
    Task<InvitationDto> UpdateAsync(int id, InvitationDto invitationDto);
    Task<bool> DeleteByIdAsync(int id);
}
