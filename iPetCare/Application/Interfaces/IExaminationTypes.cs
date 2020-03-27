using System;
using System.Threading.Tasks;
using Application.Dtos.ExaminationTypes;
using Application.Services.Utilities;

namespace Application.Interfaces
{
    public interface IExaminationTypes
    {
        Task<ServiceResponse<ExaminationTypesCreateDtoResponse>> CreateExaminationTypesAsync(ExaminationTypesCreateDtoRequest dto);
        Task<ServiceResponse<ExaminationTypesGetAllDtoResponse>> GetAllExaminationTypesAsync();
        Task<ServiceResponse<ExaminationTypeGetDtoResponse>> GetExaminationTypeAsync(int examinationTypeId);
        Task<ServiceResponse<ExaminationTypeUpdateDtoResponse>> UpdateExaminationTypeAsync(int examinationTypeId, ExaminationTypeUpdateDtoRequest dto);
        Task<ServiceResponse> DeleteExaminationTypeAsync(int examinationTypeId);
    }
}
