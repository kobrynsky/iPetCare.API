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
        Task<ServiceResponse<InstitutionsGetInstitutionDtoResponse>> GetInstitutionAsync(Guid institutionId);
        Task<ServiceResponse<InstitutionsGetInstitutionsDtoResponse>> GetInstitutionsAsync();
        Task<ServiceResponse<InstitutionsUpdateInstitutionDtoResponse>> UpdateInstitutionAsync(Guid institutionId, InstitutionsUpdateInstitutionDtoRequest dto);
        Task<ServiceResponse<InstitutionsCreateInstitutionDtoResponse>> CreateInstitutionAsync(InstitutionsCreateInstitutionDtoRequest dto);
        Task<ServiceResponse> DeleteInstitutionAsync(Guid institutionId);
    }
}
