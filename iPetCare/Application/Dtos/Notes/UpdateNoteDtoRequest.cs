using System;
using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Notes
{
    public class UpdateNoteDtoRequest
    {
        [Required(ErrorMessage = "Pole jest wymagane")]
        [MaxLength(512, ErrorMessage = "Długość nie może być większa, niż 512 znaków")]
        public string Payload { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ImportantDate { get; set; }
        public Guid PetId { get; set; }
    }
}
