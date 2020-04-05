using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Application.Dtos.ExaminationParameterValues
{
    public class UpdateExaminationParameterValueDtoRequest
    {
        [Required]
        [Range(0, float.MaxValue, ErrorMessage = "Please enter valid float Number")]
        public float Value { get; set; }
        [Required]
        public int ExaminationParameterId { get; set; }
        [Required]
        public Guid ExaminationId { get; set; }
    }
}
