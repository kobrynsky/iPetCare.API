using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.ExaminationParameters
{
    public class ExaminationParametersCreateExaminationParameterDtoRequest
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public float UpperLimit { get; set; }
        [Required]
        public float LowerLimit { get; set; }
        [Required]
        public int ExaminationTypeId { get; set; }
    }
}
