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

namespace Application.Services
{
    public class ExaminationParameterService : Service, IExaminationParameterService
    {
        public ExaminationParameterService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public async Task<ServiceResponse<CreateExaminationParameterDtoResponse>> CreateExaminationParameterAsync(CreateExaminationParameterDtoRequest dto)
        {
            if (CurrentlyLoggedUser == null)
                return new ServiceResponse<CreateExaminationParameterDtoResponse>(HttpStatusCode.Unauthorized);

            if (CurrentlyLoggedUser.Role != Role.Administrator)
                return new ServiceResponse<CreateExaminationParameterDtoResponse>(HttpStatusCode.Forbidden);

            var examinationType = await Context.ExaminationTypes.FindAsync(dto.ExaminationTypeId);

            if (examinationType == null)
                return new ServiceResponse<CreateExaminationParameterDtoResponse>(HttpStatusCode.BadRequest, "Nie istnieje take badanie w bazie danych");

            if (await Context.ExaminationParameters.Where(x => x.Name == dto.Name && x.ExaminationTypeId == dto.ExaminationTypeId).AnyAsync())
                return new ServiceResponse<CreateExaminationParameterDtoResponse>(HttpStatusCode.BadRequest, "Podany parametr już istnieje dla tego badania");

            ExaminationParameter examinationParameter = Mapper.Map<ExaminationParameter>(dto);

            Context.ExaminationParameters.Add(examinationParameter);
            int result = await Context.SaveChangesAsync();

            var responseDto = Mapper.Map<CreateExaminationParameterDtoResponse>(examinationParameter);

            return result > 0
                ? new ServiceResponse<CreateExaminationParameterDtoResponse>(HttpStatusCode.OK, responseDto)
                : new ServiceResponse<CreateExaminationParameterDtoResponse>(HttpStatusCode.BadRequest, "Nie nastąpiło zapisanie do bazy danych");
        }

        public async Task<ServiceResponse<GetAllExaminationParametersDtoResponse>> GetAllExaminationParametersAsync()
        {
            if (CurrentlyLoggedUser == null)
                return new ServiceResponse<GetAllExaminationParametersDtoResponse>(HttpStatusCode.Unauthorized);

            var examinationParameter = await Context.ExaminationParameters.ToListAsync();

            var dto = new GetAllExaminationParametersDtoResponse()
            {
                ExaminationParameters = Mapper.Map<List<ExaminationParameterForGetAllExaminationParametersDtoResponse>>(examinationParameter)
            };

            return new ServiceResponse<GetAllExaminationParametersDtoResponse>(HttpStatusCode.OK, dto);
        }

        public async Task<ServiceResponse<GetExaminationParameterDtoResponse>> GetExaminationParameterAsync(int examinationParameterId)
        {
            if (CurrentlyLoggedUser == null)
                return new ServiceResponse<GetExaminationParameterDtoResponse>(HttpStatusCode.Unauthorized);

            var examinationParameter = await Context.ExaminationParameters.FindAsync(examinationParameterId);
            if (examinationParameter == null)
                return new ServiceResponse<GetExaminationParameterDtoResponse>(HttpStatusCode.NotFound);

            var dto = Mapper.Map<GetExaminationParameterDtoResponse>(examinationParameter);

            return new ServiceResponse<GetExaminationParameterDtoResponse>(HttpStatusCode.OK, dto);
        }

        public async Task<ServiceResponse<UpdateExaminationParameterDtoResponse>> UpdateExaminationParameterAsync(int examinationParameterId, UpdateExaminationParameterDtoRequest dto)
        {
            if (CurrentlyLoggedUser == null)
                return new ServiceResponse<UpdateExaminationParameterDtoResponse>(HttpStatusCode.Unauthorized);

            if (CurrentlyLoggedUser.Role != Role.Administrator)
                return new ServiceResponse<UpdateExaminationParameterDtoResponse>(HttpStatusCode.Forbidden);

            var examinationParameter = await Context.ExaminationParameters.FindAsync(examinationParameterId);
            if (examinationParameter == null)
                return new ServiceResponse<UpdateExaminationParameterDtoResponse>(HttpStatusCode.NotFound);

            var examinationType = Context.ExaminationTypes.Find(examinationParameter.ExaminationTypeId);

            if (await Context.ExaminationParameters.Where(x => x.Name == dto.Name && x.ExaminationTypeId == examinationParameter.ExaminationTypeId && x.Id != examinationParameter.Id).AnyAsync())
                return new ServiceResponse<UpdateExaminationParameterDtoResponse>(HttpStatusCode.BadRequest, "Istnieje już taki parametr dla tego badania");

            examinationParameter.Name = dto.Name;
            examinationParameter.UpperLimit = dto.UpperLimit;
            examinationParameter.LowerLimit = dto.LowerLimit;

            int result = await Context.SaveChangesAsync();
            if (result > 0)
            {
                var responseDto = Mapper.Map<UpdateExaminationParameterDtoResponse>(examinationParameter);
                return new ServiceResponse<UpdateExaminationParameterDtoResponse>(HttpStatusCode.OK, responseDto);
            }

            return new ServiceResponse<UpdateExaminationParameterDtoResponse>(HttpStatusCode.BadRequest, "Nie nastąpiło zapisanie do bazy danych");
        }

        public async Task<ServiceResponse> DeleteExaminationParameterAsync(int examinationParameterId)
        {
            if (CurrentlyLoggedUser == null)
                return new ServiceResponse(HttpStatusCode.Unauthorized);

            if (CurrentlyLoggedUser.Role != Role.Administrator)
                return new ServiceResponse(HttpStatusCode.Forbidden);

            var examinationParameter = await Context.ExaminationParameters.FindAsync(examinationParameterId);
            if (examinationParameter == null)
                return new ServiceResponse(HttpStatusCode.NotFound);

            Context.ExaminationParameters.Remove(examinationParameter);
            int result = await Context.SaveChangesAsync();

            if (result > 0)
                return new ServiceResponse(HttpStatusCode.OK);

            return new ServiceResponse(HttpStatusCode.BadRequest, "Parametr nie został usunięty");
        }
    }
}
