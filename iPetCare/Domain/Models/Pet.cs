using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class Pet
    {
        public Guid Id { get; set; }

        public string ImageUrl { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        public float Weight { get; set; }

        public float Height { get; set; }

        public Gender Gender { get; set; }

        public DateTime BirthDate { get; set; }

        public int RaceId { get; set; }
        public virtual Race Race { get; set; }

        public virtual ICollection<Examination> Examinations { get; set; }

        public virtual ICollection<ImportantDatePet> ImportantDatePets { get; set; }

        public virtual ICollection<Note> Notes { get; set; }

        public virtual ICollection<OwnerPet> OwnerPets { get; set; }

        public virtual ICollection<VetPet> VetPets { get; set; }

        public virtual ICollection<Request> Requests { get; set; }
    }
}
