
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class ExaminationType
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(512)]
        public string Name { get; set; }
        
        public int SpeciesId { get; set; }
        public virtual Species Species { get; set; }

        public virtual ICollection<Examination> Examinations { get; set; }

        public virtual ICollection<ExaminationParameter> ExaminationParameters { get; set; }
    }
}
