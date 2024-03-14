using FilmForge.Common.Enum;
using FilmForge.Entities.EntityModels.Base;

namespace FilmForge.Entities.EntityModels;

public class Invitation : BaseEntity
{
    public bool HasAccepted { get; set; }
    public InvitationType InvitationType { get; set; }
    public int MovieId { get; set; }
    public Movie Movie { get; set; }
    public int ActorId { get; set; }
    public Actor Actor { get; set; }
}