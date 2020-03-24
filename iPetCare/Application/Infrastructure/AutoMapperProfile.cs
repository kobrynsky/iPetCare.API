using System;
using System.Collections.Generic;
using System.Text;
using Application.Dtos.Users;
using Application.Dtos.Races;
using AutoMapper;
using Domain.Models;
using Application.Dtos.Species;

namespace Application.Infrastructure
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            MapsForUser();
            MapsForRaces();
            MapsForSpecies();
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
        
        private void MapsForSpecies()
        {
            CreateMap<Species, SpeciesDetailGetAllDtoResponse>();
            CreateMap<Species, SpeciesGetDtoResponse>();
            CreateMap<Species, SpeciesDeleteDtoResponse>();
            CreateMap<Species, SpeciesUpdateDtoResponse>();
            CreateMap<Race, RaceDetailsGetDtoResponse>();
        }
    }
}
