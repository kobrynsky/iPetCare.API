using Application.Dtos.Pet;
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
            MapsForPets();
            MapsForRaces();
            MapsForSpecies();
        }

        private void MapsForUser()
        {
            CreateMap<ApplicationUser, UserGetAllDtoResponse>();
        }

        private void MapsForPets()
        {
            CreateMap<Pet, PetForPetsGetPetsDtoResponse>()
                .ForMember(d => d.Race, opt => opt.MapFrom(s => s.Race.Name));
            CreateMap<PetsCreatePetDtoRequest, Pet>();
            CreateMap<Pet, PetsCreatePetDtoResponse>()
                .ForMember(d => d.Race, opt => opt.MapFrom(s => s.Race.Name));
            CreateMap<Pet, PetsGetPetDtoResponse>()
                .ForMember(d => d.Race, opt => opt.MapFrom(s => s.Race.Name));
            CreateMap<PetsUpdatePetDtoRequest, PetsUpdatePetDtoResponse>();
            CreateMap<PetsUpdatePetDtoRequest, Pet>();
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
