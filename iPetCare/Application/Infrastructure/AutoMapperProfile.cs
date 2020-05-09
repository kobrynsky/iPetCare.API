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
using Application.Dtos.Invitations;
using Application.Dtos.Owners;
using Application.Dtos.Vets;
using Application.Dtos.Notes;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

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
            MapsForInvitations();
            MapsForNotes();
        }

        private void MapsForUser()
        {
            CreateMap<ApplicationUser, UserForGetAllUsersDtoResponse>();
            CreateMap<ApplicationUser, EditProfileDtoResponse>()
                .ForMember(d => d.Specialization, opt =>
                {
                    opt.PreCondition(s => s.Vet != null);
                    opt.MapFrom(s => s.Vet.Specialization);
                })
                .ForMember(d => d.PlaceOfResidence, opt =>
                {
                    opt.PreCondition(s => s.Owner != null);
                    opt.MapFrom(s => s.Owner.PlaceOfResidence);
                });

            CreateMap<Vet, VetForGetVetsDto>()
                .ForMember(d => d.Role, opt => opt.MapFrom(s => s.User.Role))
                .ForMember(d => d.Email, opt => opt.MapFrom(s => s.User.Email))
                .ForMember(d => d.FirstName, opt => opt.MapFrom(s => s.User.FirstName))
                .ForMember(d => d.LastName, opt => opt.MapFrom(s => s.User.LastName))
                .ForMember(d => d.Institutions,
                    opt => opt.MapFrom(s => s.InstitutionVets.Select(iv => iv.Institution).ToList()));

            CreateMap<Owner, OwnerForGetOwnersDto>()
                .ForMember(d => d.Role, opt => opt.MapFrom(s => s.User.Role))
                .ForMember(d => d.Email, opt => opt.MapFrom(s => s.User.Email))
                .ForMember(d => d.FirstName, opt => opt.MapFrom(s => s.User.FirstName))
                .ForMember(d => d.LastName, opt => opt.MapFrom(s => s.User.LastName))
                .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Id))
                .ForMember(d => d.UserId, opt => opt.MapFrom(s => s.UserId));
        }

        private void MapsForPets()
        {
            CreateMap<Pet, PetForGetPetsDtoResponse>();
            CreateMap<Pet, PetForGetMyPetsDtoResponse>();
            CreateMap<Pet, PetForGetUserPetsDtoResponse>()
                .ForMember(d => d.Race, opt => opt.MapFrom(s => s.Race.Name))
                .ForMember(d => d.Species, opt => opt.MapFrom(s => s.Race.Species.Name));
            CreateMap<Pet, PetForGetSharedPetsDtoResponse>();
            CreateMap<CreatePetDtoRequest, Pet>();
            CreateMap<Pet, CreatePetDtoResponse>()
                .ForMember(d => d.Race, opt => opt.MapFrom(s => s.Race.Name));
            CreateMap<Pet, GetPetDtoResponse>();
            CreateMap<UpdatePetDtoRequest, UpdatePetDtoResponse>();
            CreateMap<UpdatePetDtoRequest, Pet>();
        }

        private void MapsForRaces()
        {
            CreateMap<Race, RaceForGetAllRacesDtoResponse>();
            CreateMap<Race, GetRaceDtoResponse>();
            CreateMap<Race, DeleteRaceDtoResponse>();
            CreateMap<Race, UpdateRaceDtoResponse>();
            CreateMap<Species, SpeciesDetailsGetDtoResponse>();
        }

        private void MapsForSpecies()
        {
            CreateMap<Species, SpeciesForGetAllSpeciesDtoResponse>();
            CreateMap<Species, GetSpeciesDtoResponse>();
            CreateMap<Species, DeleteSpeciesDtoResponse>();
            CreateMap<Species, UpdateSpeciesDtoResponse>();
            CreateMap<Race, RaceForGetSpeciesDtoResponse>();
        }


        private void MapsForInstitutions()
        {
            CreateMap<ApplicationUser, UserForGetInstitutionDtoResponse>();
            CreateMap<Institution, GetInstitutionDtoResponse>()
                .ForMember(d => d.Vets, opt => opt.MapFrom(i => i.InstitutionVets.Select(x => x.Vet.User)));
            CreateMap<Institution, InstitutionForGetInstitutionDtoResponse>();
            CreateMap<CreateInstitutionDtoRequest, Institution>();
            CreateMap<CreateInstitutionDtoResponse, Institution>();
            CreateMap<Institution, CreateInstitutionDtoResponse>();
            CreateMap<Institution, UpdateInstitutionDtoResponse>();
            CreateMap<Institution, InstitutionForGetVetsDto>();
            CreateMap<Institution, SignUpDtoResponse>();
        }

        private void MapsForExaminationTypes()
        {
            CreateMap<ExaminationType, ExaminationTypeForGetAllExaminationTypesDtoResponse>();
            CreateMap<ExaminationType, ExaminationTypeForGetExaminationTypesByPetIdResponse>();
            CreateMap<ExaminationType, UpdateExaminationTypeDtoResponse>();
            CreateMap<ExaminationParameter, ExaminationParameterDetailsForExaminationTypeGetDtoResponse>();
        }

        private void MapsForExaminationParameters()
        {
            CreateMap<ExaminationParameter, CreateExaminationParameterDtoResponse>();
            CreateMap<ExaminationParameter, ExaminationParameterForGetAllExaminationParametersDtoResponse>();
            CreateMap<ExaminationParameter, GetExaminationParameterDtoResponse>();
            CreateMap<ExaminationType, ExaminationTypeForGetExaminationParameterDtoResponse>();
            CreateMap<ExaminationParameter, UpdateExaminationParameterDtoResponse>();
            CreateMap<CreateExaminationParameterDtoRequest, ExaminationParameter>();
        }

        private void MapsForExaminations()
        {
            CreateMap<Examination, ExaminationForGetAllExaminationsDtoResponse>();
            CreateMap<Examination, GetExaminationDtoResponse>();
            CreateMap<ExaminationType, ExaminationTypeForGetExaminationDtoResponse>();
            CreateMap<Pet, PetForGetExaminationDtoResponse>();
            CreateMap<ExaminationParameterValue, ExaminationParameterValueForGetExaminationDtoResponse>();
            CreateMap<ExaminationParameter, ExaminationParameterForGetExaminationDtoResponse>();
            CreateMap<Examination, UpdateExaminationDtoResponse>();
        }
        private void MapsForExaminationParameterValues()
        {
            CreateMap<ExaminationParameterValue, CreateExaminationParameterValueDtoResponse>();
            CreateMap<ExaminationParameterValue, ExaminationParameterValueForGetAllExaminationParametersValuesDtoResponse>();
            CreateMap<ExaminationParameterValue, GetExaminationParameterValueDtoResponse>();
            CreateMap<Examination, ExaminationForGetExaminationParametersValuesDtoResponse>();
            CreateMap<ExaminationParameter, ExaminationParameterForGetExaminationParametersValuesDtoResponse>();
            CreateMap<ExaminationParameterValue, UpdateExaminationParameterValueDtoResponse>();
            CreateMap<CreateExaminationParameterValueDtoRequest, ExaminationParameterValue>();
        }

        private void MapsForInvitations()
        {
            CreateMap<Request, CreateInvitationDtoResponse>();
            CreateMap<Request, ChangeStatusInvitationDtoResponse>();
        }

        private void MapsForNotes()
        {
            CreateMap<Note, CreateNoteDtoResponse>();
            CreateMap<Note, NoteForGetAllNotesDtoResponse>();
            CreateMap<Note, GetNoteDtoResponse>();
            CreateMap<Note, UpdateNoteDtoResponse>();
            CreateMap<CreateNoteDtoRequest, Note>();
            CreateMap<ApplicationUser, UserForGetNoteDtoResponse>();
            CreateMap<Pet, PetForGetNoteDtoResponse>();
            CreateMap<Note, NoteForGetImportantDatesDtoResponse>();
        }
    }
}
