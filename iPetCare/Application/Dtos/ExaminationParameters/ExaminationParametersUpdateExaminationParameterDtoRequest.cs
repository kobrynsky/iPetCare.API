using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Dtos.ExaminationParameters
{
    public class ExaminationParametersUpdateExaminationParameterDtoRequest
    {
        public string Name { get; set; }
        public float UpperLimit { get; set; }
        public float LowerLimit { get; set; }
        public int ExaminationTypeId { get; set; }
    }
}
