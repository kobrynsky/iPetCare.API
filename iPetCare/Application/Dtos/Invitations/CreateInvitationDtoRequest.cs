using System;
using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Invitations
{
    public class CreateInvitationDtoRequest
    {
        public Guid Id { get; set; }

        [MaxLength(255, ErrorMessage = "Długość nie może być większa, niż 255 znaków")]
        public string UserId { get; set; }

        public Guid PetId { get; set; }
    }
}
