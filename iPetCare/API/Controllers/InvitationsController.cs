using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Application.Interfaces;
using Application.Dtos.Invitations;
using Application.Services.Utilities;
using Domain.Models;
using API.Security;

namespace API.Controllers
{
    public class InvitationsController : BaseController
    {
        private readonly IInvitationService _invitationService;

        public InvitationsController(IInvitationService invitationService)
        {
            _invitationService = invitationService;
        }

        [Produces(typeof(ServiceResponse<CreateInvitationDtoResponse>))]
        [AuthorizeRoles(Role.Vet, Role.Owner)]
        [HttpPost]
        public async Task<IActionResult> CreateInvitation(CreateInvitationDtoRequest dto)
        {
            var response = await _invitationService.CreateInvitationAsync(dto);
            return SendResponse(response);
        }

        [Produces(typeof(ServiceResponse))]
        [AuthorizeRoles(Role.Vet, Role.Owner)]
        [HttpDelete("{InvitationId}")]
        public async Task<IActionResult> DeleteInvitation(Guid InvitationId)
        {
            var response = await _invitationService.DeleteInvitationAsync(InvitationId);
            return SendResponse(response);
        }
    }
}
