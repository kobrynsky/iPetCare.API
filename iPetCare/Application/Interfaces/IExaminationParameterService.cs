using System;
using System.Threading.Tasks;
using Application.Dtos.ExaminationParameters;
using Application.Services.Utilities;

namespace Application.Interfaces
{
    public interface IExaminationParameterService
    {
        Task<ServiceResponse<ExaminationParametersCreateExaminationParameterDtoResponse>> CreateExaminationParameterAsync(ExaminationParametersCreateExaminationParameterDtoRequest dto);
    }
}
