﻿using System;
using System.Threading.Tasks;
using Application.Dtos.Invitations;
using Application.Services.Utilities;

namespace Application.Interfaces
{
    public interface IInvitationService
    {
        Task<ServiceResponse<CreateInvitationDtoResponse>> CreateInvitationAsync(CreateInvitationDtoRequest dto);
        Task<ServiceResponse> DeleteInvitationAsync(Guid invitationId);
        Task<ServiceResponse<ChangeStatusInvitationDtoResponse>> ChangeInvitationStatusAsync(ChangeStatusInvitationDtoRequest dto, Guid invitationId);
    }
}