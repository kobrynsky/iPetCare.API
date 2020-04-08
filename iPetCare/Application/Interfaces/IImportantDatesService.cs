using System;
using System.Threading.Tasks;
using Application.Dtos.ImportantDates;
using Application.Services.Utilities;

namespace Application.Interfaces
{
    public interface IImportantDatesService
    {
        Task<ServiceResponse<CreateImportantDateDtoResponse>> CreateImportantDateAsync(CreateImportantDateDtoRequest dto);
        Task<ServiceResponse<GetAllImportantDatesDtoResponse>> GetAllImportantDatesAsync();
        Task<ServiceResponse<GetImportantDateDtoResponse>> GetImportantDateAsync(Guid importantDateId);
        Task<ServiceResponse<UpdateImportantDateDtoResponse>> UpdateImportantDateAsync(Guid importantDateId, UpdateImportantDateDtoRequest dto);
        Task<ServiceResponse> DeleteImportantDateAsync(Guid importantDateId);
    }
}
