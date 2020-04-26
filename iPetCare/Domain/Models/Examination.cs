using System;
using System.Collections.Generic;

namespace Domain.Models
{
    public class Examination
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }

        public int ExaminationTypeId { get; set; }
        public virtual ExaminationType ExaminationType { get; set; }
        public string Content { get; set; }
        public Guid PetId { get; set; }
        public virtual Pet Pet { get; set; }

        public virtual ICollection<ExaminationParameterValue> ExaminationParameterValues { get; set; }
    }
}
