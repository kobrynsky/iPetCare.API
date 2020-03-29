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
        Task<ServiceResponse<ExaminationsGetExaminationDtoResponse>> GetExaminationAsync(string petId, int examinationTypeId);
        Task<ServiceResponse<ExaminationsUpdateExaminationDtoResponse>> UpdateExaminationAsync(string petId, int examinationTypeId, ExaminationsUpdateExaminationDtoRequest dto);
        Task<ServiceResponse> DeleteExaminationAsync(string petId, int examinationTypeId);
        Task<ServiceResponse<ExaminationsGetAllExaminationsDtoResponse>> GetPetAllExaminationsAsync(string petId);
    }
}
