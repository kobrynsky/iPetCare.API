using System;
using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Invitations
{
    public class CreateInvitationDtoRequest
    {
        public Guid PetId { get; set; }
    }
}
