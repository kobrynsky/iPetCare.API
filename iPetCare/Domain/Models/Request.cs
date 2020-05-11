using System;


namespace Domain.Models
{
    public class Request
    {
        public Guid Id { get; set; }

        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        public Guid PetId { get; set; }
        public virtual Pet Pet { get; set; }
    }
}
