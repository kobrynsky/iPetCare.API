using System;
using System.Collections.Generic;
using System.Text;
using Application.Dtos.Users;
using Application.Dtos.Races;
using AutoMapper;
using Domain.Models;

namespace Application.Infrastructure
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            MapsForUser();
            MapsForRaces();
        }

        private void MapsForUser()
        {
            CreateMap<ApplicationUser, UserGetAllDtoResponse>();
        }

        private void MapsForRaces()
        {
            CreateMap<Race, RaceDetailGetAllDtoResponse>();
            CreateMap<Race, RaceGetDtoResponse>();
            CreateMap<Race, RaceDeleteDtoResponse>();
            CreateMap<Race, RaceUpdateDtoResponse>();
            CreateMap<Species, SpeciesDetailsGetDtoResponse>();
        }
    }
}
