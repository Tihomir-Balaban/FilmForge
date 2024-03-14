using D = FilmForge.Models.Dtos;

namespace FilmForge.Models.Statics.ActorDto
{
    public static class ActorDtoHelpers
    {
        public static bool CheckFeeChange(this ulong feeOne, ulong feeTwo)
        {
            return feeOne != feeTwo;
        }
    }
}