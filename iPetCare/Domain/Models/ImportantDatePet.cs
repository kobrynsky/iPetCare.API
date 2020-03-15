
using System;

namespace Domain.Models
{
    public class ImportantDatePet
    {
        public Guid ImportantDateId { get; set; }   
        public virtual ImportantDate ImportantDate { get; set; }

        public Guid PetId { get; set; }
        public virtual Pet Pet { get; set; }
    }
}
