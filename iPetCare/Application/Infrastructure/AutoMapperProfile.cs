using Application.Dtos.Pet;
using Application.Dtos.Users;
using Application.Dtos.Races;
using AutoMapper;
using Domain.Models;
using Application.Dtos.Species;
using Application.Dtos.ExaminationTypes;
using Application.Dtos.ExaminationParameters;

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
            MapsForExaminationTypes();
            MapsForExaminationParameters();
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
            CreateMap<Species, SpeciesGetSpeciesDtoResponse>();
            CreateMap<Species, SpeciesDeleteSpeciesDtoResponse>();
            CreateMap<Species, SpeciesUpdateSpeciesDtoResponse>();
            CreateMap<Race, RaceDetailsGetDtoResponse>();
        }

        private void MapsForExaminationTypes()
        {
            CreateMap<ExaminationType, ExaminationTypesDetailGetAllDtoResponse>();
            CreateMap<ExaminationType, ExaminationTypesGetExaminationTypeDtoResponse>();
            CreateMap<ExaminationType, ExaminationTypesUpdateExaminationTypeDtoResponse>();
            CreateMap<ExaminationParameter, ExaminationParameterDetailsForExaminationTypeGetDtoResponse>();
        }

        private void MapsForExaminationParameters()
        {
            CreateMap<ExaminationParameter, ExaminationParametersDetailsGetAllDtoResponse>();
            CreateMap<ExaminationParameter, ExaminationParametersGetExaminationParameterDtoResponse>();
            CreateMap<ExaminationType, ExaminationTypeDetailsGetDtoResponse>();
            CreateMap<ExaminationParameter, ExaminationParametersUpdateExaminationParameterDtoResponse>();
        }
    }
}
