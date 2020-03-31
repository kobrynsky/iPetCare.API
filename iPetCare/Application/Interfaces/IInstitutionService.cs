using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Application.Dtos.Institutions;
using Application.Services.Utilities;

namespace Application.Interfaces
{
    public interface IInstitutionService
    {
        Task<ServiceResponse<GetInstitutionDtoResponse>> GetInstitutionAsync(Guid institutionId);
        Task<ServiceResponse<GetInstitutionsDtoResponse>> GetInstitutionsAsync();
        Task<ServiceResponse<UpdateInstitutionDtoResponse>> UpdateInstitutionAsync(Guid institutionId, UpdateInstitutionDtoRequest dto);
        Task<ServiceResponse<CreateInstitutionDtoResponse>> CreateInstitutionAsync(CreateInstitutionDtoRequest dto);
        Task<ServiceResponse> DeleteInstitutionAsync(Guid institutionId);
        Task<ServiceResponse> SignUpAsync(Guid institutionId);
        Task<ServiceResponse> SignOutAsync(Guid institutionId);
    }
}
