using System;
using System.Linq;
using System.Collections.Generic;
using System.Net;
using Application.Interfaces;
using Application.Services.Utilities;
using Application.Dtos.Examinations;
using Domain.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

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
                return new ServiceResponse<CreateExaminationDtoResponse>(HttpStatusCode.BadRequest, "Nie znaleziono zwierzaka");

            if (!CanEditExamination(pet))
                return new ServiceResponse<CreateExaminationDtoResponse>(HttpStatusCode.Forbidden);

            var examinationType = Context.ExaminationTypes.Find(dto.ExaminationTypeId);

            if(examinationType == null)
                return new ServiceResponse<CreateExaminationDtoResponse>(HttpStatusCode.BadRequest, "Nieprawidłowy typ badania");

            if (dto.Id == Guid.Empty)
                dto.Id = Guid.NewGuid();

            var examination = new Examination()
            {
                Id = dto.Id,
                Date = dto.Date,
                ExaminationTypeId = dto.ExaminationTypeId,
                PetId = dto.PetId,
                Content = dto.Content,
            };

            Context.Examinations.Add(examination);
            int result = await Context.SaveChangesAsync();

            if (result > 0)
            {
                var responseDto = new CreateExaminationDtoResponse()
                {
                    Id = examination.Id,
                    Date = examination.Date,
                    ExaminationTypeId = examination.ExaminationTypeId,
                    PetId = examination.PetId,
                    Content = dto.Content,
                };

                return new ServiceResponse<CreateExaminationDtoResponse>(HttpStatusCode.OK, responseDto);
            }

            return new ServiceResponse<CreateExaminationDtoResponse>(HttpStatusCode.BadRequest, "Wystąpił błąd podczas tworzenia badania");
        }

        public async Task<ServiceResponse> DeleteExaminationAsync(Guid petId, Guid examinationId)
        {
            if (CurrentlyLoggedUser == null)
                return new ServiceResponse(HttpStatusCode.Unauthorized);

            var pet = Context.Pets.Find(petId);

            if (pet == null)
                return new ServiceResponse(HttpStatusCode.BadRequest, "Nie znaleziono zwierzaka");

            if (!CanEditExamination(pet))
                return new ServiceResponse(HttpStatusCode.Forbidden);

            var examination = Context.Examinations.Find(examinationId);
            if (examination == null)
                return new ServiceResponse(HttpStatusCode.NotFound);

            var examinationValues = Context.ExaminationParameterValues.Where(x => x.ExaminationId == examinationId);

            if (examinationValues.Any())
                Context.ExaminationParameterValues.RemoveRange(examinationValues);

            Context.Examinations.Remove(examination);
            int result = await Context.SaveChangesAsync();

            return result > 0
                ? new ServiceResponse(HttpStatusCode.OK)
                : new ServiceResponse(HttpStatusCode.BadRequest, "Wystąpił błąd podczas usuwania badania");
        }

        public async Task<ServiceResponse<GetAllExaminationsDtoResponse>> GetAllExaminationsAsync()
        {
            if (CurrentlyLoggedUser == null)
                return new ServiceResponse<GetAllExaminationsDtoResponse>(HttpStatusCode.Unauthorized);

            var examinations = await Context.Examinations.ToListAsync();

            var dto = new GetAllExaminationsDtoResponse()
            {
                Examinations = Mapper.Map<List<ExaminationForGetAllExaminationsDtoResponse>>(examinations)
            };

            return new ServiceResponse<GetAllExaminationsDtoResponse>(HttpStatusCode.OK, dto);
        }

        public async Task<ServiceResponse<GetExaminationDtoResponse>> GetExaminationAsync(Guid examinationId)
        {
            if (CurrentlyLoggedUser == null)
                return new ServiceResponse<GetExaminationDtoResponse>(HttpStatusCode.Unauthorized);

            var examination = await Context.Examinations.FindAsync(examinationId);
            if (examination == null)
                return new ServiceResponse<GetExaminationDtoResponse>(HttpStatusCode.NotFound);

            var pet = Context.Pets.Find(examination.PetId);

            if (pet == null)
                return new ServiceResponse<GetExaminationDtoResponse>(HttpStatusCode.BadRequest, "Nie znaleziono zwierzaka");

            if (!CanEditExamination(pet))
                return new ServiceResponse<GetExaminationDtoResponse>(HttpStatusCode.Forbidden);

            var dto = Mapper.Map<GetExaminationDtoResponse>(examination);
            return new ServiceResponse<GetExaminationDtoResponse>(HttpStatusCode.OK, dto);
        }

        public async Task<ServiceResponse<GetAllExaminationsDtoResponse>> GetPetExaminationsAsync(Guid petId)
        {
            if (CurrentlyLoggedUser == null)
                return new ServiceResponse<GetAllExaminationsDtoResponse>(HttpStatusCode.Unauthorized);

            var pet = Context.Pets.Find(petId);

            if (pet == null)
                return new ServiceResponse<GetAllExaminationsDtoResponse>(HttpStatusCode.NotFound);

            if (!CanEditExamination(pet))
                return new ServiceResponse<GetAllExaminationsDtoResponse>(HttpStatusCode.Forbidden);

            var examinations = await Context.Examinations.Where(ex => ex.PetId == petId).ToListAsync();

            var dto = new GetAllExaminationsDtoResponse()
            {
                Examinations = Mapper.Map<List<ExaminationForGetAllExaminationsDtoResponse>>(examinations)
            };

            return new ServiceResponse<GetAllExaminationsDtoResponse>(HttpStatusCode.OK, dto);
        }

        public async Task<ServiceResponse<UpdateExaminationDtoResponse>> UpdateExaminationAsync(Guid petId, Guid examinationId, UpdateExaminationDtoRequest dto)
        {
            if (CurrentlyLoggedUser == null)
                return new ServiceResponse<UpdateExaminationDtoResponse>(HttpStatusCode.Unauthorized);

            var pet = Context.Pets.Find(petId);
            if (pet == null)
                return new ServiceResponse<UpdateExaminationDtoResponse>(HttpStatusCode.BadRequest, "Nie znaleziono zwierzaka");

            if (!CanEditExamination(pet))
                return new ServiceResponse<UpdateExaminationDtoResponse>(HttpStatusCode.Forbidden);

            var examination = Context.Examinations.Find(examinationId);
            var examinationType = Context.ExaminationTypes.Find(dto.ExaminationTypeId);

            if (examination == null)
                return new ServiceResponse<UpdateExaminationDtoResponse>(HttpStatusCode.NotFound);
            if (examinationType == null)
                return new ServiceResponse<UpdateExaminationDtoResponse>(HttpStatusCode.BadRequest, "Nie znaleziono typu badania");

            examination.Date = dto.Date;
            examination.ExaminationTypeId = dto.ExaminationTypeId;
            examination.PetId = dto.PetId;
            examination.Content = dto.Content;

            int result = await Context.SaveChangesAsync();
            if (result > 0)
            {
                var responseDto = Mapper.Map<UpdateExaminationDtoResponse>(examination);
                return new ServiceResponse<UpdateExaminationDtoResponse>(HttpStatusCode.OK, responseDto);
            }

            return new ServiceResponse<UpdateExaminationDtoResponse>(HttpStatusCode.BadRequest, "Wystąpił błąd podczas zapisu badania");
        }

        private bool CanEditExamination(Pet pet)
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
