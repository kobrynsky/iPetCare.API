using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Dtos.ExaminationParameterValuess
{
    public class ExaminationParameterValuessGetAllExaminationParametersValuesDtoResponse
    {
        public List<ExaminationParameterValuesDetailsGetAllDtoResponse> ExaminationParametersValues { get; set; }
    }

    public class ExaminationParameterValuesDetailsGetAllDtoResponse
    {
        public Guid Id { get; set; }
        public float Value { get; set; }
        public int ExaminationParameterId { get; set; }
        public Guid ExaminationId { get; set; }
    }
}
