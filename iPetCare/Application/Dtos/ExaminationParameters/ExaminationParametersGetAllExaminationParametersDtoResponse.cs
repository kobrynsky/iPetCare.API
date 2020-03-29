using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Dtos.ExaminationParameters
{
    public class ExaminationParametersGetAllExaminationParametersDtoResponse
    {
        public List<ExaminationParametersDetailsGetAllDtoResponse> ExaminationParameters { get; set; }
    }

    public class ExaminationParametersDetailsGetAllDtoResponse
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public float UpperLimit { get; set; }
        public float LowerLimit { get; set; }
        public int ExaminationTypeId { get; set; }
    }
}
