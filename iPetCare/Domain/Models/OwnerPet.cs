
using System;

namespace Domain.Models
{
    public class OwnerPet
    {
        public Guid OwnerId { get; set; }
        public virtual Owner Owner { get; set; }

        public Guid PetId { get; set; }
        public virtual Pet Pet { get; set; }
        public bool MainOwner { get; set; }
    }
}
