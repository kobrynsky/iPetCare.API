using System.Threading.Tasks;
using Application.Dtos.ExaminationParameters;
using Application.Services.Utilities;

namespace Application.Interfaces
{
    public interface IExaminationParameterService
    {
        Task<ServiceResponse<CreateExaminationParameterDtoResponse>> CreateExaminationParameterAsync(CreateExaminationParameterDtoRequest dto);
        Task<ServiceResponse<GetAllExaminationParametersDtoResponse>> GetAllExaminationParametersAsync();
        Task<ServiceResponse<GetExaminationParameterDtoResponse>> GetExaminationParameterAsync(int examinationParameterId);
        Task<ServiceResponse<UpdateExaminationParameterDtoResponse>> UpdateExaminationParameterAsync(int examinationParameterId, UpdateExaminationParameterDtoRequest dto);
        Task<ServiceResponse> DeleteExaminationParameterAsync(int examinationParameterId);
    }
}
