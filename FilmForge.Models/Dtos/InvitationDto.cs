using FilmForge.Common.Enum;

namespace FilmForge.Models.Dtos
{
    public class InvitationDto
    {
        public int Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
        public bool HasAccepted { get; set; }
        public InvitationType InvitationType { get; set; }

        public int MovieId { get; set; }
        public MovieDto Movie { get; set; }
        public int ActorId { get; set; }

        public ActorDto Actor { get; set; }
    }
}