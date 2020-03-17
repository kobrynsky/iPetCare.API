using System;
using System.Collections.Generic;

namespace Domain.Models
{
    public class Owner
    {
        public Guid Id { get; set; }

        public string PlaceOfResidence { get; set; }

        public virtual ICollection<OwnerPet> OwnerPets { get; set; }

        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}
