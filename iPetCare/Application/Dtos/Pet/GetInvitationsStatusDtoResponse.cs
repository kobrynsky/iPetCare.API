using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Dtos.Pet
{
    public class GetInvitationsStatusDtoResponse
    {
        public List<InvitationStatusForGetInvitationsStatusDtoResponse> InvitationsStatus { get; set; }
    }

    public class InvitationStatusForGetInvitationsStatusDtoResponse
    {
        public Guid InvitationId { get; set; }
        public UserForGetInvitationsStatusDtoResponse User { get; set; }
        public PetForGetInvitationsStatusDtoResponse Pet { get; set; }
        public bool Pending { get; set; }
    }

    public class PetForGetInvitationsStatusDtoResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }

    public class UserForGetInvitationsStatusDtoResponse
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
    }
}
