using FilmForge.Entities.EntityModels;

namespace FilmForge.Models.Profiles
{
    public class AutoMappingProfiles : Profile
    {
        public AutoMappingProfiles()
        {
            CreateReverseMaps();
            CreateDto2EntityMaps();
            CreateEntity2DtoMaps();
        }

        private void CreateReverseMaps()
        {
            CreateMap<UserDto, User>()
                .ReverseMap();

            CreateMap<UserDto[], User[]>()
                .ReverseMap();
        }

        private void CreateDto2EntityMaps()
        {
        }

        private void CreateEntity2DtoMaps()
        {
        }
    }
}
