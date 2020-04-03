using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Dtos.Invitations
{
    public class CreateInvitationDtoRequest
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public Guid PetId { get; set; }
    }
}
