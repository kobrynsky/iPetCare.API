using System;
using System.Threading.Tasks;
using Application.Dtos.Examinations;
using Application.Services.Utilities;

namespace Application.Interfaces
{
    public interface IExaminationService
    {
        Task<ServiceResponse<CreateExaminationDtoResponse>> CreateExaminationAsync(CreateExaminationDtoRequest dto);
        Task<ServiceResponse<GetAllExaminationsDtoResponse>> GetAllExaminationsAsync();
        Task<ServiceResponse<GetExaminationDtoResponse>> GetExaminationAsync(Guid examinationId);
        Task<ServiceResponse<UpdateExaminationDtoResponse>> UpdateExaminationAsync(Guid petId, Guid examinationId, UpdateExaminationDtoRequest dto);
        Task<ServiceResponse> DeleteExaminationAsync(Guid petId, Guid examinationId);
        Task<ServiceResponse<GetAllExaminationsDtoResponse>> GetPetExaminationsAsync(Guid petId);
    }
}
