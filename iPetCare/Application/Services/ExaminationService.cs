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

        public async Task<ServiceResponse<ExaminationsCreateExaminationDtoResponse>> CreateExaminationAsync(ExaminationsCreateExaminationDtoRequest dto)
        {
            if (CurrentlyLoggedUser == null)
                return new ServiceResponse<ExaminationsCreateExaminationDtoResponse>(HttpStatusCode.Unauthorized);

            var examinationType = Context.ExaminationTypes.Find(dto.ExaminationTypeId);

            if(examinationType == null)
                return new ServiceResponse<ExaminationsCreateExaminationDtoResponse>(HttpStatusCode.NotFound);

            var examination = new Examination()
            {
                Date = dto.Date,
                ExaminationTypeId = dto.ExaminationTypeId,
                PetId = Guid.Parse(dto.PetId)
            };

            if (dto.NoteId != null)
                examination.NoteId = dto.NoteId;
            else
                examination.NoteId = null;

            Context.Examinations.Add(examination);
            int result = await Context.SaveChangesAsync();

            if (result > 0)
            {
                var responseDto = new ExaminationsCreateExaminationDtoResponse()
                {
                    Id = examination.Id,
                    Date = examination.Date,
                    ExaminationTypeId = examination.ExaminationTypeId,
                    NoteId = examination.NoteId,
                    PetId = examination.PetId
                };

                return new ServiceResponse<ExaminationsCreateExaminationDtoResponse>(HttpStatusCode.OK, responseDto);
            }

            return new ServiceResponse<ExaminationsCreateExaminationDtoResponse>(HttpStatusCode.BadRequest);
        }

        public async Task<ServiceResponse> DeleteExaminationAsync(string petId, string examinationId)
        {
            if (CurrentlyLoggedUser == null)
                return new ServiceResponse(HttpStatusCode.Unauthorized);

            var pet = Context.Pets.Find(Guid.Parse(petId));

            if (pet == null)
                return new ServiceResponse(HttpStatusCode.NotFound);

            if (!CheckIfCanEditExamination(pet))
            {
                return new ServiceResponse(HttpStatusCode.Forbidden);
            }

            var examination = Context.Examinations.Find(Guid.Parse(examinationId));
            if (examination == null)
                return new ServiceResponse(HttpStatusCode.NotFound);

            Context.Examinations.Remove(examination);
            int result = await Context.SaveChangesAsync();

            if (result > 0)
                return new ServiceResponse(HttpStatusCode.OK);

            return new ServiceResponse(HttpStatusCode.BadRequest);
        }

        public async Task<ServiceResponse<ExaminationsGetAllExaminationsDtoResponse>> GetAllExaminationsAsync()
        {
            if (CurrentlyLoggedUser == null)
                return new ServiceResponse<ExaminationsGetAllExaminationsDtoResponse>(HttpStatusCode.Unauthorized);

            var examinations = await Context.Examinations.ToListAsync();

            var dto = new ExaminationsGetAllExaminationsDtoResponse()
            {
                Examinations = Mapper.Map<List<ExaminationsDetailGetAllDtoResponse>>(examinations)
            };

            return new ServiceResponse<ExaminationsGetAllExaminationsDtoResponse>(HttpStatusCode.OK, dto);
        }

        public async Task<ServiceResponse<ExaminationsGetExaminationDtoResponse>> GetExaminationAsync(string petId, string examinationId)
        {
            if (CurrentlyLoggedUser == null)
                return new ServiceResponse<ExaminationsGetExaminationDtoResponse>(HttpStatusCode.Unauthorized);

            var pet = Context.Pets.Find(Guid.Parse(petId));

            if (pet == null)
                return new ServiceResponse<ExaminationsGetExaminationDtoResponse>(HttpStatusCode.NotFound);

            if (!CheckIfCanEditExamination(pet))
            {
                return new ServiceResponse<ExaminationsGetExaminationDtoResponse>(HttpStatusCode.Forbidden);
            }

            var examination = await Context.Examinations.FindAsync(Guid.Parse(examinationId));
            if (examination == null)
                return new ServiceResponse<ExaminationsGetExaminationDtoResponse>(HttpStatusCode.NotFound);

            var dto = Mapper.Map<ExaminationsGetExaminationDtoResponse>(examination);

            var parameterValues = await Context.ExaminationParameterValues.Where(param => param.ExaminationParameter.ExaminationTypeId == examination.ExaminationTypeId).ToListAsync();

            if (parameterValues != null)
                dto.ParameterValues = Mapper.Map<List<ExaminationParameteterValueDetailsGetDtoResponse>>(parameterValues);

            return new ServiceResponse<ExaminationsGetExaminationDtoResponse>(HttpStatusCode.OK, dto);
        }

        public async Task<ServiceResponse<ExaminationsGetAllExaminationsDtoResponse>> GetPetExaminationsAsync(string petId)
        {
            if (CurrentlyLoggedUser == null)
                return new ServiceResponse<ExaminationsGetAllExaminationsDtoResponse>(HttpStatusCode.Unauthorized);

            var pet = Context.Pets.Find(Guid.Parse(petId));

            if (pet == null)
                return new ServiceResponse<ExaminationsGetAllExaminationsDtoResponse>(HttpStatusCode.NotFound);

            if (!CheckIfCanEditExamination(pet))
            {
                return new ServiceResponse<ExaminationsGetAllExaminationsDtoResponse>(HttpStatusCode.Forbidden);
            }

            var examinations = await Context.Examinations.ToListAsync();
            var filteredExaminations = examinations.Where(ex => ex.PetId.ToString().Equals(petId)).ToList();

            var dto = new ExaminationsGetAllExaminationsDtoResponse()
            {
                Examinations = Mapper.Map<List<ExaminationsDetailGetAllDtoResponse>>(filteredExaminations)
            };

            return new ServiceResponse<ExaminationsGetAllExaminationsDtoResponse>(HttpStatusCode.OK, dto);
        }

        public async Task<ServiceResponse<ExaminationsUpdateExaminationDtoResponse>> UpdateExaminationAsync(string petId, string examinationId, ExaminationsUpdateExaminationDtoRequest dto)
        {
            if (CurrentlyLoggedUser == null)
                return new ServiceResponse<ExaminationsUpdateExaminationDtoResponse>(HttpStatusCode.Unauthorized);

            var pet = Context.Pets.Find(Guid.Parse(petId));
            if (pet == null)
                return new ServiceResponse<ExaminationsUpdateExaminationDtoResponse>(HttpStatusCode.NotFound);

            if (!CheckIfCanEditExamination(pet))
            {
                return new ServiceResponse<ExaminationsUpdateExaminationDtoResponse>(HttpStatusCode.Forbidden);
            }

            var examination = Context.Examinations.Find(Guid.Parse(examinationId));
            var examinationType = Context.ExaminationTypes.Find(dto.ExaminationTypeId);
            Note note = null;
            if (dto.NoteId != null)
                note = Context.Notes.Find(dto.NoteId);

            if (examination == null)
                return new ServiceResponse<ExaminationsUpdateExaminationDtoResponse>(HttpStatusCode.NotFound);
            if (examinationType == null)
                return new ServiceResponse<ExaminationsUpdateExaminationDtoResponse>(HttpStatusCode.NotFound);
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
                var responseDto = Mapper.Map<ExaminationsUpdateExaminationDtoResponse>(examination);
                return new ServiceResponse<ExaminationsUpdateExaminationDtoResponse>(HttpStatusCode.OK, responseDto);
            }

            return new ServiceResponse<ExaminationsUpdateExaminationDtoResponse>(HttpStatusCode.BadRequest);
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
                else
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
