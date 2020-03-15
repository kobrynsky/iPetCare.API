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

        public Guid NoteId { get; set; }
        public virtual Note Note { get; set; }

        public virtual ICollection<ExaminationParameterValue> ExaminationParameterValues { get; set; }
    }
}
