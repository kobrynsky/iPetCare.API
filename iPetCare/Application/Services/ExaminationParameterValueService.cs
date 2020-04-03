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

        public Task<ServiceResponse<ExaminationParameterValuesCreateExaminationParameterValueDtoResponse>> CreateExaminationParameterValueAsync(ExaminationParameterValuesCreateExaminationParameterValueDtoRequest dto)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponse> DeleteExaminationParameterValueAsync(int examinationParameterId)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponse<ExaminationParameterValuesGetAllExaminationParametersValuesDtoResponse>> GetAllExaminationParametersValuesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponse<ExaminationParameterValuesGetExaminationParameterValueDtoResponse>> GetExaminationParameterValueAsync(int examinationParameterId)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponse<ExaminationParameterValuesUpdateExaminationParameterValueDtoResponse>> UpdateExaminationParameterValueAsync(int examinationParameterId, ExaminationParameterValuesUpdateExaminationParameterValueDtoRequest dto)
        {
            throw new NotImplementedException();
        }
    }
}
