using System;
using System.Threading.Tasks;
using Application.Dtos.ExaminationTypes;
using Application.Services.Utilities;

namespace Application.Interfaces
{
    public interface IExaminationTypeService
    {
        Task<ServiceResponse<ExaminationTypesCreateExaminationTypeDtoResponse>> CreateExaminationTypeAsync(ExaminationTypesCreateExaminationTypeDtoRequest dto);
        Task<ServiceResponse<ExaminationTypesGetAllExaminationTypesDtoResponse>> GetAllExaminationTypesAsync();
        Task<ServiceResponse<ExaminationTypesGetExaminationTypeDtoResponse>> GetExaminationTypeAsync(int examinationTypeId);
        Task<ServiceResponse<ExaminationTypesUpdateExaminationTypeDtoResponse>> UpdateExaminationTypeAsync(int examinationTypeId, ExaminationTypesUpdateExaminationTypeDtoRequest dto);
        Task<ServiceResponse> DeleteExaminationTypeAsync(int examinationTypeId);
    }
}
