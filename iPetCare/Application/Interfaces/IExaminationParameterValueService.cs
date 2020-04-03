using System;
using System.Threading.Tasks;
using Application.Dtos.ExaminationParameterValues;
using Application.Services.Utilities;

namespace Application.Interfaces
{
    public interface IExaminationParameterValueService
    {
        Task<ServiceResponse<ExaminationParameterValuesCreateExaminationParameterValueDtoResponse>> CreateExaminationParameterValueAsync(ExaminationParameterValuesCreateExaminationParameterValueDtoRequest dto);
        Task<ServiceResponse<ExaminationParameterValuesGetAllExaminationParametersValuesDtoResponse>> GetAllExaminationParametersValuesAsync();
        Task<ServiceResponse<ExaminationParameterValuesGetExaminationParameterValueDtoResponse>> GetExaminationParameterValueAsync(int examinationParameterId);
        Task<ServiceResponse<ExaminationParameterValuesUpdateExaminationParameterValueDtoResponse>> UpdateExaminationParameterValueAsync(int examinationParameterId, ExaminationParameterValuesUpdateExaminationParameterValueDtoRequest dto);
        Task<ServiceResponse> DeleteExaminationParameterValueAsync(int examinationParameterId);
    }
}
