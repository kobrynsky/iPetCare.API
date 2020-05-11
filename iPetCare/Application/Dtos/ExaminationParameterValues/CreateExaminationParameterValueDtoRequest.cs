using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.ExaminationParameterValues
{
    public class CreateExaminationParameterValueDtoRequest
    {
        [Required(ErrorMessage = "Pole jest wymagane")]
        [Range(float.MinValue, float.MaxValue, ErrorMessage = "Prosze wprowadzić wartość w formacie liczbowym")]
        public float Value { get; set; }
        public int ExaminationParameterId { get; set; }
        public Guid ExaminationId { get; set; }
    }
}
