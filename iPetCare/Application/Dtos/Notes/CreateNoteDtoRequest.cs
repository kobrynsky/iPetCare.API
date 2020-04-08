using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Notes
{
    public class CreateNoteDtoRequest
    {
        [Required]
        [MaxLength(512)]
        public string Payload { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid PetId { get; set; }
        [Required]
        public string UserId { get; set; }
    }
}
