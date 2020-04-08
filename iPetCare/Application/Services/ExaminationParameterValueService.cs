using System;
using System.Linq;
using System.Net;
using System.Collections.Generic;
using Application.Interfaces;
using Application.Services.Utilities;
using Application.Dtos.ExaminationParameters;
using Domain.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Application.Dtos.ExaminationParameterValues;

namespace Application.Services
{
    public class ExaminationParameterValueService : Service, IExaminationParameterValueService
    {
        public ExaminationParameterValueService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public async Task<ServiceResponse<CreateExaminationParameterValueDtoResponse>> CreateExaminationParameterValueAsync(CreateExaminationParameterValueDtoRequest dto)
        {
            if (CurrentlyLoggedUser == null)
                return new ServiceResponse<CreateExaminationParameterValueDtoResponse>(HttpStatusCode.Unauthorized);

            var examination = await Context.Examinations.FindAsync(dto.ExaminationId);
            if (examination == null)
                return new ServiceResponse<CreateExaminationParameterValueDtoResponse>(HttpStatusCode.BadRequest, "Nie istnieje takie badanie w bazie danych");

            var pet = await Context.Pets.FindAsync(examination.PetId);

            if (pet == null)
                return new ServiceResponse<CreateExaminationParameterValueDtoResponse>(HttpStatusCode.BadRequest, "Nie znaleziono zwierzaka");

            if (!CanEditExaminationParameterValueAsync(pet))
                return new ServiceResponse<CreateExaminationParameterValueDtoResponse>(HttpStatusCode.Forbidden);

            var examinationParameter = await Context.ExaminationParameters.FindAsync(dto.ExaminationParameterId);

            if (examinationParameter == null)
                return new ServiceResponse<CreateExaminationParameterValueDtoResponse>(HttpStatusCode.BadRequest, "Nie istnieje taki parametr w bazie danych");

            ExaminationParameterValue examinationParameterValue = Mapper.Map<ExaminationParameterValue>(dto);

            Context.ExaminationParameterValues.Add(examinationParameterValue);
            int result = await Context.SaveChangesAsync();

            var responseDto = Mapper.Map<CreateExaminationParameterValueDtoResponse>(examinationParameterValue);

            return result > 0
                ? new ServiceResponse<CreateExaminationParameterValueDtoResponse>(HttpStatusCode.OK, responseDto)
                : new ServiceResponse<CreateExaminationParameterValueDtoResponse>(HttpStatusCode.BadRequest, "Nie nastąpiło zapisanie do bazy danych");
        }

        public async Task<ServiceResponse> DeleteExaminationParameterValueAsync(Guid examinationParameterValueId)
        {
            if (CurrentlyLoggedUser == null)
                return new ServiceResponse(HttpStatusCode.Unauthorized);

            var examinationParameterValue = await Context.ExaminationParameterValues.FindAsync(examinationParameterValueId);
            if (examinationParameterValue == null)
                return new ServiceResponse(HttpStatusCode.NotFound);

            var examination = await Context.Examinations.FindAsync(examinationParameterValue.ExaminationId);
            if (examination == null)
                return new ServiceResponse(HttpStatusCode.BadRequest, "Nie istnieje takie badanie w bazie danych");

            var pet = await Context.Pets.FindAsync(examination.PetId);

            if (pet == null)
                return new ServiceResponse(HttpStatusCode.BadRequest, "Nie znaleziono zwierzaka");

            if (!CanEditExaminationParameterValueAsync(pet))
                return new ServiceResponse(HttpStatusCode.Forbidden);

            Context.ExaminationParameterValues.Remove(examinationParameterValue);
            int result = await Context.SaveChangesAsync();

            if (result > 0)
                return new ServiceResponse(HttpStatusCode.OK);

            return new ServiceResponse(HttpStatusCode.BadRequest, "Wartość parametru nie został usunięty");
        }

        public async Task<ServiceResponse<GetAllExaminationParametersValuesDtoResponse>> GetAllExaminationParametersValuesAsync()
        {
            if (CurrentlyLoggedUser == null)
                return new ServiceResponse<GetAllExaminationParametersValuesDtoResponse>(HttpStatusCode.Unauthorized);

            if (CurrentlyLoggedUser.Role != Role.Administrator)
                return new ServiceResponse<GetAllExaminationParametersValuesDtoResponse>(HttpStatusCode.Forbidden);

            var examinationParametersValues = await Context.ExaminationParameterValues.ToListAsync();

            var dto = new GetAllExaminationParametersValuesDtoResponse()
            {
                ExaminationParametersValues = Mapper.Map<List<ExaminationParameterValueForGetAllExaminationParametersValuesDtoResponse>>(examinationParametersValues)
            };

            return new ServiceResponse<GetAllExaminationParametersValuesDtoResponse>(HttpStatusCode.OK, dto);
        }

        public async Task<ServiceResponse<GetExaminationParameterValueDtoResponse>> GetExaminationParameterValueAsync(Guid examinationParameterValueId)
        {
            if (CurrentlyLoggedUser == null)
                return new ServiceResponse<GetExaminationParameterValueDtoResponse>(HttpStatusCode.Unauthorized);

            var examinationParameterValue = await Context.ExaminationParameterValues.FindAsync(examinationParameterValueId);
            if (examinationParameterValue == null)
                return new ServiceResponse<GetExaminationParameterValueDtoResponse>(HttpStatusCode.NotFound);

            var examination = await Context.Examinations.FindAsync(examinationParameterValue.ExaminationId);
            if (examination == null)
                return new ServiceResponse<GetExaminationParameterValueDtoResponse>(HttpStatusCode.BadRequest, "Nie istnieje takie badanie w bazie danych");

            var pet = await Context.Pets.FindAsync(examination.PetId);

            if (pet == null)
                return new ServiceResponse<GetExaminationParameterValueDtoResponse>(HttpStatusCode.BadRequest, "Nie znaleziono zwierzaka");

            if (!CanEditExaminationParameterValueAsync(pet))
                return new ServiceResponse<GetExaminationParameterValueDtoResponse>(HttpStatusCode.Forbidden);

            var examinationParameter = await Context.ExaminationParameters.FindAsync(examinationParameterValue.ExaminationParameterId);
            if (examinationParameter == null)
                return new ServiceResponse<GetExaminationParameterValueDtoResponse>(HttpStatusCode.NotFound);

            examinationParameterValue.Examination = examination;
            examinationParameterValue.ExaminationParameter = examinationParameter;

            var dto = Mapper.Map<GetExaminationParameterValueDtoResponse>(examinationParameterValue);

            return new ServiceResponse<GetExaminationParameterValueDtoResponse>(HttpStatusCode.OK, dto);
        }

        public async Task<ServiceResponse<UpdateExaminationParameterValueDtoResponse>> UpdateExaminationParameterValueAsync(Guid examinationParameterValueId, UpdateExaminationParameterValueDtoRequest dto)
        {
            if (CurrentlyLoggedUser == null)
                return new ServiceResponse<UpdateExaminationParameterValueDtoResponse>(HttpStatusCode.Unauthorized);

            if(examinationParameterValueId == Guid.Empty)
                return new ServiceResponse<UpdateExaminationParameterValueDtoResponse>(HttpStatusCode.BadRequest, "Nieprawidłowy indeks");

            var examinationParameterValue = await Context.ExaminationParameterValues.FindAsync(examinationParameterValueId);
            if (examinationParameterValue == null)
                return new ServiceResponse<UpdateExaminationParameterValueDtoResponse>(HttpStatusCode.NotFound);

            var examination = await Context.Examinations.FindAsync(examinationParameterValue.ExaminationId);
            if (examination == null)
                return new ServiceResponse<UpdateExaminationParameterValueDtoResponse>(HttpStatusCode.BadRequest, "Nie istnieje takie badanie w bazie danych");

            var pet = await Context.Pets.FindAsync(examination.PetId);

            if (pet == null)
                return new ServiceResponse<UpdateExaminationParameterValueDtoResponse>(HttpStatusCode.BadRequest, "Nie znaleziono zwierzaka");

            if (!CanEditExaminationParameterValueAsync(pet))
                return new ServiceResponse<UpdateExaminationParameterValueDtoResponse>(HttpStatusCode.Forbidden);

            examinationParameterValue.ExaminationId = dto.ExaminationId;
            examinationParameterValue.ExaminationParameterId = dto.ExaminationParameterId;
            examinationParameterValue.Value = dto.Value;

            int result = await Context.SaveChangesAsync();
            if (result > 0)
            {
                var responseDto = Mapper.Map<UpdateExaminationParameterValueDtoResponse>(examinationParameterValue);
                return new ServiceResponse<UpdateExaminationParameterValueDtoResponse>(HttpStatusCode.OK, responseDto);
            }

            return new ServiceResponse<UpdateExaminationParameterValueDtoResponse>(HttpStatusCode.BadRequest, "Nie nastąpiło zapisanie do bazy danych");
        }

        private bool CanEditExaminationParameterValueAsync(Pet pet)
        {
            if (CurrentlyLoggedUser.Role != Role.Administrator)
            {
                if (CurrentlyLoggedUser.Role == Role.Owner)
                {
                    var owners = pet.OwnerPets.Where(ownerpet => ownerpet.OwnerId == CurrentlyLoggedUser.Owner.Id);
                    if (!owners.Any())
                        return false;
                }
                if (CurrentlyLoggedUser.Role == Role.Vet)
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
