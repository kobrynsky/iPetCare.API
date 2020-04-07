using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Application.Dtos.ExaminationParameterValues
{
    public class UpdateExaminationParameterValueDtoRequest
    {
        [Required]
        [Range(float.MinValue, float.MaxValue, ErrorMessage = "Please enter valid float Number")]
        public float Value { get; set; }
        public int ExaminationParameterId { get; set; }
        public Guid ExaminationId { get; set; }
    }
}
