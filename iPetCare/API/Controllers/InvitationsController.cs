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
        [HttpDelete("{invitationId}")]
        public async Task<IActionResult> DeleteInvitation(Guid invitationId)
        {
            var response = await _invitationService.DeleteInvitationAsync(invitationId);
            return SendResponse(response);
        }

        [Produces(typeof(ServiceResponse))]
        [AuthorizeRoles(Role.Vet, Role.Owner)]
        [HttpPut("{invitationId}/accept")]
        public async Task<IActionResult> AcceptInvitation(Guid invitationId)
        {
            var response = await _invitationService.AcceptInvitationAsync(invitationId);
            return SendResponse(response);
        }

        [Produces(typeof(ServiceResponse))]
        [AuthorizeRoles(Role.Vet, Role.Owner)]
        [HttpPut("{invitationId}/decline")]
        public async Task<IActionResult> DeclineInvitation(Guid invitationId)
        {
            var response = await _invitationService.DeclineInvitationAsync(invitationId);
            return SendResponse(response);
        }

        [Produces(typeof(ServiceResponse))]
        [AuthorizeRoles(Role.Vet, Role.Owner)]
        [HttpDelete("{invitationId}/revoke")]
        public async Task<IActionResult> RevokeInvitation(Guid invitationId)
        {
            var response = await _invitationService.RevokeInvitationAsync(invitationId);
            return SendResponse(response);
        }

        [Produces(typeof(ServiceResponse))]
        [AuthorizeRoles(Role.Vet, Role.Owner)]
        [HttpDelete("delete-access/{userId}/{petId}")]
        public async Task<IActionResult> DeleteAccess(string userId, Guid petId)
        {
            var response = await _invitationService.DeleteAccessAsync(userId, petId);
            return SendResponse(response);
        }

    }
}
