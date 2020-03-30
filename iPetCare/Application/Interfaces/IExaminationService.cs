using System;
using System.Threading.Tasks;
using Application.Dtos.Examinations;
using Application.Services.Utilities;

namespace Application.Interfaces
{
    public interface IExaminationService
    {
        Task<ServiceResponse<ExaminationsCreateExaminationDtoResponse>> CreateExaminationAsync(ExaminationsCreateExaminationDtoRequest dto);
        Task<ServiceResponse<ExaminationsGetAllExaminationsDtoResponse>> GetAllExaminationsAsync();
        Task<ServiceResponse<ExaminationsGetExaminationDtoResponse>> GetExaminationAsync(string petId, string examinationId);
        Task<ServiceResponse<ExaminationsUpdateExaminationDtoResponse>> UpdateExaminationAsync(string petId, string examinationId, ExaminationsUpdateExaminationDtoRequest dto);
        Task<ServiceResponse> DeleteExaminationAsync(string petId, string examinationId);
        Task<ServiceResponse<ExaminationsGetAllExaminationsDtoResponse>> GetPetExaminationsAsync(string petId);
    }
}
