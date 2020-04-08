using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Dtos.Notes
{
    public class GetNoteDtoResponse
    {
        public Guid Id { get; set; }
        public string Payload { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid PetId { get; set; }
        public string UserId { get; set; }
    }
}
