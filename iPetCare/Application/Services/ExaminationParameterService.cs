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

namespace Application.Services
{
    public class ExaminationParameterService : Service, IExaminationParameterService
    {
        public ExaminationParameterService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public async Task<ServiceResponse<ExaminationParametersCreateExaminationParameterDtoResponse>> CreateExaminationParameterAsync(ExaminationParametersCreateExaminationParameterDtoRequest dto)
        {
            if (CurrentlyLoggedUser == null)
                return new ServiceResponse<ExaminationParametersCreateExaminationParameterDtoResponse>(HttpStatusCode.Unauthorized);

            if (CurrentlyLoggedUser.Role != Role.Administrator)
                return new ServiceResponse<ExaminationParametersCreateExaminationParameterDtoResponse>(HttpStatusCode.Forbidden);

            var examinationType = await Context.ExaminationTypes.FindAsync(dto.ExaminationTypeId);

            if (examinationType == null)
                return new ServiceResponse<ExaminationParametersCreateExaminationParameterDtoResponse>(HttpStatusCode.BadRequest, "Nie istnieje take badanie w bazie danych");

            if (await Context.ExaminationParameters.Where(x => x.Name == dto.Name && x.ExaminationTypeId == dto.ExaminationTypeId).AnyAsync())
                return new ServiceResponse<ExaminationParametersCreateExaminationParameterDtoResponse>(HttpStatusCode.BadRequest, "Podany parametr już istnieje dla tego badania");

            ExaminationParameter examinationParameter = Mapper.Map<ExaminationParameter>(dto);

            Context.ExaminationParameters.Add(examinationParameter);
            int result = await Context.SaveChangesAsync();

            var responseDto = Mapper.Map<ExaminationParametersCreateExaminationParameterDtoResponse>(examinationParameter);

            return result > 0
                ? new ServiceResponse<ExaminationParametersCreateExaminationParameterDtoResponse>(HttpStatusCode.OK, responseDto)
                : new ServiceResponse<ExaminationParametersCreateExaminationParameterDtoResponse>(HttpStatusCode.BadRequest, "Nie nastąpiło zapisanie do bazy danych");
        }

        public async Task<ServiceResponse<ExaminationParametersGetAllExaminationParametersDtoResponse>> GetAllExaminationParametersAsync()
        {
            if (CurrentlyLoggedUser == null)
                return new ServiceResponse<ExaminationParametersGetAllExaminationParametersDtoResponse>(HttpStatusCode.Unauthorized);

            var examinationParameter = await Context.ExaminationParameters.ToListAsync();

            var dto = new ExaminationParametersGetAllExaminationParametersDtoResponse()
            {
                ExaminationParameters = Mapper.Map<List<ExaminationParametersDetailsGetAllDtoResponse>>(examinationParameter)
            };

            return new ServiceResponse<ExaminationParametersGetAllExaminationParametersDtoResponse>(HttpStatusCode.OK, dto);
        }

        public async Task<ServiceResponse<ExaminationParametersGetExaminationParameterDtoResponse>> GetExaminationParameterAsync(int examinationParameterId)
        {
            if (CurrentlyLoggedUser == null)
                return new ServiceResponse<ExaminationParametersGetExaminationParameterDtoResponse>(HttpStatusCode.Unauthorized);

            var examinationParameter = await Context.ExaminationParameters.FindAsync(examinationParameterId);
            if (examinationParameter == null)
                return new ServiceResponse<ExaminationParametersGetExaminationParameterDtoResponse>(HttpStatusCode.NotFound);

            var dto = Mapper.Map<ExaminationParametersGetExaminationParameterDtoResponse>(examinationParameter);

            return new ServiceResponse<ExaminationParametersGetExaminationParameterDtoResponse>(HttpStatusCode.OK, dto);
        }

        public async Task<ServiceResponse<ExaminationParametersUpdateExaminationParameterDtoResponse>> UpdateExaminationParameterAsync(int examinationParameterId, ExaminationParametersUpdateExaminationParameterDtoRequest dto)
        {
            if (CurrentlyLoggedUser == null)
                return new ServiceResponse<ExaminationParametersUpdateExaminationParameterDtoResponse>(HttpStatusCode.Unauthorized);

            if (CurrentlyLoggedUser.Role != Role.Administrator)
                return new ServiceResponse<ExaminationParametersUpdateExaminationParameterDtoResponse>(HttpStatusCode.Forbidden);

            var examinationParameter = await Context.ExaminationParameters.FindAsync(examinationParameterId);
            if (examinationParameter == null)
                return new ServiceResponse<ExaminationParametersUpdateExaminationParameterDtoResponse>(HttpStatusCode.NotFound);

            var examinationType = Context.ExaminationTypes.Find(examinationParameter.ExaminationTypeId);

            if (await Context.ExaminationParameters.Where(x => x.Name == dto.Name && x.ExaminationTypeId == examinationParameter.ExaminationTypeId && x.Id != examinationParameter.Id).AnyAsync())
                return new ServiceResponse<ExaminationParametersUpdateExaminationParameterDtoResponse>(HttpStatusCode.BadRequest, "Istnieje już taki parametr dla tego badania");

            examinationParameter.Name = dto.Name;
            examinationParameter.UpperLimit = dto.UpperLimit;
            examinationParameter.LowerLimit = dto.LowerLimit;

            int result = await Context.SaveChangesAsync();
            if (result > 0)
            {
                var responseDto = Mapper.Map<ExaminationParametersUpdateExaminationParameterDtoResponse>(examinationParameter);
                return new ServiceResponse<ExaminationParametersUpdateExaminationParameterDtoResponse>(HttpStatusCode.OK, responseDto);
            }

            return new ServiceResponse<ExaminationParametersUpdateExaminationParameterDtoResponse>(HttpStatusCode.BadRequest, "Nie nastąpiło zapisanie do bazy danych");
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
