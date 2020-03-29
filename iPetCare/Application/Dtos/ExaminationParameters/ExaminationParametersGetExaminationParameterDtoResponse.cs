using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Dtos.ExaminationParameters
{
    public class ExaminationParametersGetExaminationParameterDtoResponse
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public float UpperLimit { get; set; }
        public float LowerLimit { get; set; }
        public int ExaminationTypeId { get; set; }
        public ExaminationTypeDetailsGetDtoResponse ExaminationType { get; set; }
    }

    public class ExaminationTypeDetailsGetDtoResponse
    {
        public string Name { get; set; }
        public int Id { get; set; }
    }
}
