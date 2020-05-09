using System;
using System.Threading.Tasks;
using Application.Dtos.Invitations;
using Application.Services.Utilities;

namespace Application.Interfaces
{
    public interface IInvitationService
    {
        Task<ServiceResponse<CreateInvitationDtoResponse>> CreateInvitationAsync(CreateInvitationDtoRequest dto);
        Task<ServiceResponse> DeleteInvitationAsync(Guid invitationId);
        Task<ServiceResponse> AcceptInvitationAsync(Guid invitationId);
        Task<ServiceResponse> DeclineInvitationAsync(Guid invitationId);
        Task<ServiceResponse> DeleteAccessAsync(string userId, Guid petId);
        Task<ServiceResponse> RevokeInvitationAsync(Guid invitationId);
    }
}
