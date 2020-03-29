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
            var examinationType = Context.ExaminationTypes.Find(dto.ExaminationTypeId);

            if (examinationType == null)
                return new ServiceResponse<ExaminationParametersCreateExaminationParameterDtoResponse>(HttpStatusCode.BadRequest, "Nie istnieje take badanie w bazie danych");

            if (await Context.ExaminationParameters.Where(x => x.Name == dto.Name && x.ExaminationTypeId == dto.ExaminationTypeId).AnyAsync())
                return new ServiceResponse<ExaminationParametersCreateExaminationParameterDtoResponse>(HttpStatusCode.BadRequest, "Podany parametr już istnieje dla tego badania");

            if (CurrentlyLoggedUser == null)
                return new ServiceResponse<ExaminationParametersCreateExaminationParameterDtoResponse>(HttpStatusCode.Unauthorized);

            if (CurrentlyLoggedUser.Role != Role.Administrator)
                return new ServiceResponse<ExaminationParametersCreateExaminationParameterDtoResponse>(HttpStatusCode.Forbidden);

            var examinationParameter = new ExaminationParameter()
            {
                Name = dto.Name,
                UpperLimit = dto.UpperLimit,
                LowerLimit = dto.LowerLimit,
                ExaminationTypeId = dto.ExaminationTypeId
            };

            Context.ExaminationParameters.Add(examinationParameter);
            int result = await Context.SaveChangesAsync();

            var responseDto = new ExaminationParametersCreateExaminationParameterDtoResponse()
            {
                Id = examinationParameter.Id,
                Name = examinationParameter.Name,
                UpperLimit = examinationParameter.UpperLimit,
                LowerLimit = examinationParameter.LowerLimit,
                ExaminationTypeId = examinationParameter.ExaminationTypeId
            };

            return result > 0
                ? new ServiceResponse<ExaminationParametersCreateExaminationParameterDtoResponse>(HttpStatusCode.OK, responseDto)
                : new ServiceResponse<ExaminationParametersCreateExaminationParameterDtoResponse>(HttpStatusCode.BadRequest);
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
                return new ServiceResponse<ExaminationParametersUpdateExaminationParameterDtoResponse>(HttpStatusCode.BadRequest, "Nie ma takiego parametru");

            var examinationType = Context.ExaminationTypes.Find(dto.ExaminationTypeId);

            if (examinationType == null)
                return new ServiceResponse<ExaminationParametersUpdateExaminationParameterDtoResponse>(HttpStatusCode.BadRequest, "Nie ma takiego badania");

            examinationParameter.Name = dto.Name;
            examinationParameter.ExaminationTypeId = dto.ExaminationTypeId;
            examinationParameter.UpperLimit = dto.UpperLimit;
            examinationParameter.LowerLimit = dto.LowerLimit;

            int result = await Context.SaveChangesAsync();
            if (result > 0)
            {
                var responseDto = Mapper.Map<ExaminationParametersUpdateExaminationParameterDtoResponse>(examinationParameter);
                return new ServiceResponse<ExaminationParametersUpdateExaminationParameterDtoResponse>(HttpStatusCode.OK, responseDto);
            }

            if ((await Context.ExaminationParameters.Where(x => x.Name == dto.Name && x.ExaminationTypeId == dto.ExaminationTypeId).AnyAsync()) && result == 0)
                return new ServiceResponse<ExaminationParametersUpdateExaminationParameterDtoResponse>(HttpStatusCode.BadRequest, "Nie nastąpiła żadna zmiana");

            return new ServiceResponse<ExaminationParametersUpdateExaminationParameterDtoResponse>(HttpStatusCode.BadRequest);
        }

        public async Task<ServiceResponse> DeleteExaminationParameterAsync(int examinationParameterId)
        {
            if (CurrentlyLoggedUser == null)
                return new ServiceResponse(HttpStatusCode.Unauthorized);

            if (CurrentlyLoggedUser.Role != Role.Administrator)
                return new ServiceResponse(HttpStatusCode.Forbidden);

            var examinationParameter = Context.ExaminationParameters.Find(examinationParameterId);
            if (examinationParameter == null)
                return new ServiceResponse(HttpStatusCode.NotFound);

            Context.ExaminationParameters.Remove(examinationParameter);
            int result = await Context.SaveChangesAsync();

            if (result > 0)
                return new ServiceResponse(HttpStatusCode.OK);

            return new ServiceResponse(HttpStatusCode.BadRequest);
        }
    }
}
