using System;
using System.Threading.Tasks;
using Application.Dtos.ExaminationParameterValues;
using Application.Services.Utilities;

namespace Application.Interfaces
{
    public interface IExaminationParameterValueService
    {
        Task<ServiceResponse<CreateExaminationParameterValueDtoResponse>> CreateExaminationParameterValueAsync(CreateExaminationParameterValueDtoRequest dto);
        Task<ServiceResponse<GetAllExaminationParametersValuesDtoResponse>> GetAllExaminationParametersValuesAsync();
        Task<ServiceResponse<GetExaminationParameterValueDtoResponse>> GetExaminationParameterValueAsync(Guid examinationParameterValueId);
        Task<ServiceResponse<UpdateExaminationParameterValueDtoResponse>> UpdateExaminationParameterValueAsync(Guid examinationParameterValueId, UpdateExaminationParameterValueDtoRequest dto);
        Task<ServiceResponse> DeleteExaminationParameterValueAsync(Guid examinationParameterValueId);
        Task<ServiceResponse<GetAllExaminationParametersValuesDtoResponse>> GetExaminationParameterValueByExaminatinIdAsync(Guid examinationId);
    }
}
