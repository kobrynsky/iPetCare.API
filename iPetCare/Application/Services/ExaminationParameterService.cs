using System;
using System.Linq;
using System.Net;
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
    }
}
