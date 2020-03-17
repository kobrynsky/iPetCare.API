using System;

namespace Domain.Models
{
    public class VetPet
    {
        public Guid VetId { get; set; }
        public virtual Vet Vet { get; set; }

        public Guid PetId { get; set; }
        public virtual Pet Pet { get; set; }
    }
}
