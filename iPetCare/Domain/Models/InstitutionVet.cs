
using System;

namespace Domain.Models
{
    public class InstitutionVet
    {
        public Guid InstitutionId { get; set; }
        public virtual Institution Institution { get; set; }

        public Guid VetId { get; set; }
        public virtual Vet Vet { get; set; }
    }
}
