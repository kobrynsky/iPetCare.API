using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Dtos.ExaminationParameterValues
{
    public class ExaminationParameterValuesUpdateExaminationParameterValueDtoResponse
    {
        public Guid Id { get; set; }
        public float Value { get; set; }
        public int ExaminationParameterId { get; set; }
        public Guid ExaminationId { get; set; }
    }
}
