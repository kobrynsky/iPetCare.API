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
        Task<ServiceResponse<ExaminationsGetExaminationDtoResponse>> GetExaminationAsync(Guid petId, Guid examinationId);
        Task<ServiceResponse<ExaminationsUpdateExaminationDtoResponse>> UpdateExaminationAsync(Guid petId, Guid examinationId, ExaminationsUpdateExaminationDtoRequest dto);
        Task<ServiceResponse> DeleteExaminationAsync(Guid petId, Guid examinationId);
        Task<ServiceResponse<ExaminationsGetAllExaminationsDtoResponse>> GetPetExaminationsAsync(Guid petId);
    }
}
