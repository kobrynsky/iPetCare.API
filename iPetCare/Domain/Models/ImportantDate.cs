using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class ImportantDate
    {
        public Guid Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Title { get; set; }

        public DateTime Date { get; set; }

        public Guid NoteId { get; set; }
        public virtual Note Note { get; set; }

        public virtual ICollection<ImportantDatePet> ImportantDatePets { get; set; }
    }
}
