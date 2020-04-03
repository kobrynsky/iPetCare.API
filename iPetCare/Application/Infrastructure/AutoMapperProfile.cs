using System.Linq;
using Application.Dtos.Institutions;
using Application.Dtos.Pet;
using Application.Dtos.Users;
using Application.Dtos.Races;
using AutoMapper;
using Domain.Models;
using Application.Dtos.Species;
using Application.Dtos.ExaminationTypes;
using Application.Dtos.ExaminationParameters;
using Application.Dtos.ExaminationParameterValues;
using Application.Dtos.Examinations;

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
            MapsForInstitutions();
            MapsForExaminationTypes();
            MapsForExaminationParameters();
            MapsForExaminations();
            MapsForExaminationParameterValues();
        }

        private void MapsForUser()
        {
            CreateMap<ApplicationUser, UserGetAllDtoResponse>();
        }

        private void MapsForPets()
        {
            CreateMap<Pet, PetForPetsGetPetsDtoResponse>()
                .ForMember(d => d.Race, opt => opt.MapFrom(s => s.Race.Name));
            CreateMap<Pet, PetForPetsGetMyPetsDtoResponse>()
                .ForMember(d => d.Race, opt => opt.MapFrom(s => s.Race.Name));
            CreateMap<Pet, PetForPetsGetSharedPetsDtoResponse>()
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


        private void MapsForInstitutions()
        {
            CreateMap<ApplicationUser, UserForInstitutionGetInstitutionDtoResponse>();
            CreateMap<Institution, InstitutionsGetInstitutionDtoResponse>()
                .ForMember(d => d.Vets, opt => opt.MapFrom(i => i.InstitutionVets.Select(x => x.Vet.User)));
            CreateMap<Institution, InstitutionForInstitutionGetInstitutionDtoResponse>();
            CreateMap<InstitutionsCreateInstitutionDtoRequest, Institution>();
            CreateMap<InstitutionsCreateInstitutionDtoResponse, Institution>();
            CreateMap<Institution, InstitutionsCreateInstitutionDtoResponse>();
            CreateMap<Institution, InstitutionsUpdateInstitutionDtoResponse>();
        }

        private void MapsForExaminationTypes()
        {
            CreateMap<ExaminationType, ExaminationTypesDetailGetAllDtoResponse>();
            CreateMap<ExaminationType, ExaminationTypesUpdateExaminationTypeDtoResponse>();
            CreateMap<ExaminationParameter, ExaminationParameterDetailsForExaminationTypeGetDtoResponse>();
        }

        private void MapsForExaminationParameters()
        {
            CreateMap<ExaminationParameter, ExaminationParametersCreateExaminationParameterDtoResponse>();
            CreateMap<ExaminationParameter, ExaminationParametersDetailsGetAllDtoResponse>();
            CreateMap<ExaminationParameter, ExaminationParametersGetExaminationParameterDtoResponse>();
            CreateMap<ExaminationType, ExaminationTypeDetailsGetDtoResponse>();
            CreateMap<ExaminationParameter, ExaminationParametersUpdateExaminationParameterDtoResponse>();
            CreateMap<ExaminationParametersCreateExaminationParameterDtoRequest, ExaminationParameter>();
        }
        private void MapsForExaminations()
        {
            CreateMap<Examination, ExaminationsDetailGetAllDtoResponse>();
            CreateMap<Examination, ExaminationsGetExaminationDtoResponse>();
            CreateMap<Examination, ExaminationsUpdateExaminationDtoResponse>(); 
            CreateMap<Examination, ExaminationParameteterValueDetailsGetDtoResponse>();
        }
        private void MapsForExaminationParameterValues()
        {
            CreateMap<ExaminationParameterValue, ExaminationParameterValuesCreateExaminationParameterValueDtoResponse>();
            CreateMap<ExaminationParameterValue, ExaminationParameterValuesDetailsGetAllDtoResponse>();
            CreateMap<ExaminationParameterValue, ExaminationParameterValuesGetExaminationParameterValueDtoResponse>();
            CreateMap<ExaminationParameter, ExaminationParameterDetailsGetDtoResponse>();
            CreateMap<Examination, ExaminationDetailsGetDtoResponse>();
            CreateMap<ExaminationParameterValue, ExaminationParametersUpdateExaminationParameterDtoResponse>();
            CreateMap<ExaminationParameterValuesCreateExaminationParameterValueDtoRequest, ExaminationParameterValue>();
        }
    }
}
