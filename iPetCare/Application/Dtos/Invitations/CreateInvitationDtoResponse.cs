using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Dtos.Invitations
{
    public class CreateInvitationDtoResponse
    {
        public Guid Id { get; set; }
        public bool DidUserRequest { get; set; }
        public bool IsAccepted { get; set; }
        public string UserId { get; set; }
        public Guid PetId { get; set; }
    }
}
