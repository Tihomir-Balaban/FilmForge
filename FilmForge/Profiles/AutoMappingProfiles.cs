﻿using FilmForge.Entities.EntityModels;
using System.Text;

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
            // This is where any reverse mapable models are profiled
            // CreateMap<TSource, TDest>()
            //    .ReverseMap();
        }

        private void CreateDto2EntityMaps()
        {
            CreateMap<UserDto, User>()
                .ForMember(des => des.Password,
                           opt => opt.MapFrom(src => Encoding.UTF8.GetBytes(src.Password)))
                .ForMember(des => des.Salt,
                           opt => opt.MapFrom(src => Encoding.UTF8.GetBytes(src.Salt)));
        }

        private void CreateEntity2DtoMaps()
        {
            CreateMap<User, UserDto>()
                .ForMember(des => des.Password,
                           opt => opt.MapFrom(src => string.Empty))
                .ForMember(des => des.Salt,
                           opt => opt.MapFrom(src => string.Empty));
        }
    }
}
