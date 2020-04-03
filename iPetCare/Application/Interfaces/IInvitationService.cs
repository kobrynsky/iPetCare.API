using System.Threading.Tasks;
using Application.Dtos.Invitations;
using Application.Services.Utilities;

namespace Application.Interfaces
{
    public interface IInvitationService
    {
        Task<ServiceResponse<CreateInvitationDtoResponse>> CreateInvitationAsync(CreateInvitationDtoRequest dto);
    }
}
