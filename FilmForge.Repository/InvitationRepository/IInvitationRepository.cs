using FilmForge.Entities.EntityModels;

namespace FilmForge.Repository.InvitationRepository;

public interface IInvitationRepository
{
    Task<InvitationDto[]> GetByActorIdAsync(int actorId);
    Task<InvitationDto[]> GetByMovieIdAsync(int movieId);
    Task<InvitationDto> GetByActorIdAndMovieIdAsync(int actorId, int movieId);
    Task<InvitationDto> CreateAsync(Invitation invitation);
    Task<InvitationDto> UpdateAsync(int id, Invitation invitation);
    Task<bool> DeleteByIdAsync(int id);
}
