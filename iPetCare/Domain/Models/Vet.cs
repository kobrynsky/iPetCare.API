
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class Vet
    {
        public Guid Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Specialization { get; set; }

        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        public virtual ICollection<InstitutionVet> InstitutionVets { get; set; }

        public virtual ICollection<VetPet> VetPets { get; set; }
    }
}
