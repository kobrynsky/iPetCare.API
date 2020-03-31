using System;
using System.Linq;
using System.Collections.Generic;
using System.Net;
using Application.Interfaces;
using Application.Services.Utilities;
using Application.Dtos.Examinations;
using Domain.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Services
{
    public class ExaminationService : Service, IExaminationService
    {
        public ExaminationService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public async Task<ServiceResponse<CreateExaminationDtoResponse>> CreateExaminationAsync(CreateExaminationDtoRequest dto)
        {
            if (CurrentlyLoggedUser == null)
                return new ServiceResponse<CreateExaminationDtoResponse>(HttpStatusCode.Unauthorized);

            var pet = Context.Pets.Find(dto.PetId);

            if (pet == null)
                return new ServiceResponse<CreateExaminationDtoResponse>(HttpStatusCode.NotFound);

            if (!CheckIfCanEditExamination(pet))
            {
                return new ServiceResponse<CreateExaminationDtoResponse>(HttpStatusCode.Forbidden);
            }


            var examinationType = Context.ExaminationTypes.Find(dto.ExaminationTypeId);

            if(examinationType == null)
                return new ServiceResponse<CreateExaminationDtoResponse>(HttpStatusCode.NotFound);

            var examination = new Examination()
            {
                Date = dto.Date,
                ExaminationTypeId = dto.ExaminationTypeId,
                PetId = dto.PetId
            };

            if (dto.NoteId != null)
                examination.NoteId = dto.NoteId;
            else
                examination.NoteId = null;

            Context.Examinations.Add(examination);
            int result = await Context.SaveChangesAsync();

            if (result > 0)
            {
                var responseDto = new CreateExaminationDtoResponse()
                {
                    Id = examination.Id,
                    Date = examination.Date,
                    ExaminationTypeId = examination.ExaminationTypeId,
                    NoteId = examination.NoteId,
                    PetId = examination.PetId
                };

                return new ServiceResponse<CreateExaminationDtoResponse>(HttpStatusCode.OK, responseDto);
            }

            return new ServiceResponse<CreateExaminationDtoResponse>(HttpStatusCode.BadRequest);
        }

        public async Task<ServiceResponse> DeleteExaminationAsync(Guid petId, Guid examinationId)
        {
            if (CurrentlyLoggedUser == null)
                return new ServiceResponse(HttpStatusCode.Unauthorized);

            var pet = Context.Pets.Find(petId);

            if (pet == null)
                return new ServiceResponse(HttpStatusCode.NotFound);

            if (!CheckIfCanEditExamination(pet))
            {
                return new ServiceResponse(HttpStatusCode.Forbidden);
            }

            var examination = Context.Examinations.Find(examinationId);
            if (examination == null)
                return new ServiceResponse(HttpStatusCode.NotFound);

            Context.Examinations.Remove(examination);
            int result = await Context.SaveChangesAsync();

            if (result > 0)
                return new ServiceResponse(HttpStatusCode.OK);

            return new ServiceResponse(HttpStatusCode.BadRequest);
        }

        public async Task<ServiceResponse<GetAllExaminationsDtoResponse>> GetAllExaminationsAsync()
        {
            if (CurrentlyLoggedUser == null)
                return new ServiceResponse<GetAllExaminationsDtoResponse>(HttpStatusCode.Unauthorized);

            var examinations = await Context.Examinations.ToListAsync();

            var dto = new GetAllExaminationsDtoResponse()
            {
                Examinations = Mapper.Map<List<DetailGetAllDtoResponse>>(examinations)
            };

            return new ServiceResponse<GetAllExaminationsDtoResponse>(HttpStatusCode.OK, dto);
        }

        public async Task<ServiceResponse<GetExaminationDtoResponse>> GetExaminationAsync(Guid petId, Guid examinationId)
        {
            if (CurrentlyLoggedUser == null)
                return new ServiceResponse<GetExaminationDtoResponse>(HttpStatusCode.Unauthorized);

            var pet = Context.Pets.Find(petId);

            if (pet == null)
                return new ServiceResponse<GetExaminationDtoResponse>(HttpStatusCode.NotFound);

            if (!CheckIfCanEditExamination(pet))
            {
                return new ServiceResponse<GetExaminationDtoResponse>(HttpStatusCode.Forbidden);
            }

            var examination = await Context.Examinations.FindAsync(examinationId);
            if (examination == null)
                return new ServiceResponse<GetExaminationDtoResponse>(HttpStatusCode.NotFound);

            var dto = Mapper.Map<GetExaminationDtoResponse>(examination);

            var parameterValues = await Context.ExaminationParameterValues.Where(param => param.ExaminationParameter.ExaminationTypeId == examination.ExaminationTypeId).ToListAsync();

            if (parameterValues != null)
                dto.ParameterValues = Mapper.Map<List<ParameterValueDetailsGetDtoResponse>>(parameterValues);

            return new ServiceResponse<GetExaminationDtoResponse>(HttpStatusCode.OK, dto);
        }

        public async Task<ServiceResponse<GetAllExaminationsDtoResponse>> GetPetExaminationsAsync(Guid petId)
        {
            if (CurrentlyLoggedUser == null)
                return new ServiceResponse<GetAllExaminationsDtoResponse>(HttpStatusCode.Unauthorized);

            var pet = Context.Pets.Find(petId);

            if (pet == null)
                return new ServiceResponse<GetAllExaminationsDtoResponse>(HttpStatusCode.NotFound);

            if (!CheckIfCanEditExamination(pet))
            {
                return new ServiceResponse<GetAllExaminationsDtoResponse>(HttpStatusCode.Forbidden);
            }

            var examinations = await Context.Examinations.ToListAsync();
            var filteredExaminations = examinations.Where(ex => ex.PetId == petId).ToList();

            var dto = new GetAllExaminationsDtoResponse()
            {
                Examinations = Mapper.Map<List<DetailGetAllDtoResponse>>(filteredExaminations)
            };

            return new ServiceResponse<GetAllExaminationsDtoResponse>(HttpStatusCode.OK, dto);
        }

        public async Task<ServiceResponse<UpdateExaminationDtoResponse>> UpdateExaminationAsync(Guid petId, Guid examinationId, UpdateExaminationDtoRequest dto)
        {
            if (CurrentlyLoggedUser == null)
                return new ServiceResponse<UpdateExaminationDtoResponse>(HttpStatusCode.Unauthorized);

            var pet = Context.Pets.Find(petId);
            if (pet == null)
                return new ServiceResponse<UpdateExaminationDtoResponse>(HttpStatusCode.NotFound);

            if (!CheckIfCanEditExamination(pet))
            {
                return new ServiceResponse<UpdateExaminationDtoResponse>(HttpStatusCode.Forbidden);
            }

            var examination = Context.Examinations.Find(examinationId);
            var examinationType = Context.ExaminationTypes.Find(dto.ExaminationTypeId);
            Note note = null;
            if (dto.NoteId != null)
                note = Context.Notes.Find(dto.NoteId);

            if (examination == null)
                return new ServiceResponse<UpdateExaminationDtoResponse>(HttpStatusCode.NotFound);
            if (examinationType == null)
                return new ServiceResponse<UpdateExaminationDtoResponse>(HttpStatusCode.NotFound);
            if (note == null)
                examination.NoteId = null;
            else
                examination.NoteId = dto.NoteId;

            examination.Date = dto.Date;
            examination.ExaminationTypeId = dto.ExaminationTypeId;         
            examination.PetId = dto.PetId;

            int result = await Context.SaveChangesAsync();
            if (result > 0)
            {
                var responseDto = Mapper.Map<UpdateExaminationDtoResponse>(examination);
                return new ServiceResponse<UpdateExaminationDtoResponse>(HttpStatusCode.OK, responseDto);
            }

            return new ServiceResponse<UpdateExaminationDtoResponse>(HttpStatusCode.BadRequest);
        }

        private bool CheckIfCanEditExamination(Pet pet)
        {
            if (CurrentlyLoggedUser.Role != Role.Administrator)
            {
                if (CurrentlyLoggedUser.Role == Role.Owner)
                {
                    var owners = pet.OwnerPets.Where(ownerpet => ownerpet.OwnerId == CurrentlyLoggedUser.Owner.Id);
                    if (!owners.Any())
                        return false;
                }
                if(CurrentlyLoggedUser.Role == Role.Vet)
                {
                    var vets = pet.VetPets.Where(vetpet => vetpet.VetId == CurrentlyLoggedUser.Vet.Id);
                    if (!vets.Any())
                        return false;
                }
            }
            return true;
        }
    }
}
