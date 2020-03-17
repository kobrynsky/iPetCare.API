using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class ExaminationParameter
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        public float UpperLimit { get; set; }

        public float LowerLimit { get; set; }

        public int ExaminationTypeId { get; set; }
        public virtual ExaminationType ExaminationType { get; set; }

        public virtual ICollection<ExaminationParameterValue> ExaminationParameterValues { get; set; }
    }
}
