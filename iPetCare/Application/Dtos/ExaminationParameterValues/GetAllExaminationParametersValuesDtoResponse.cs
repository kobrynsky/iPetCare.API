using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Dtos.ExaminationParameterValues
{
    public class GetAllExaminationParametersValuesDtoResponse
    {
        public List<ExaminationParameterValueForGetAllExaminationParametersValuesDtoResponse> ExaminationParametersValues { get; set; }
    }

    public class ExaminationParameterValueForGetAllExaminationParametersValuesDtoResponse
    {
        public Guid Id { get; set; }
        public float Value { get; set; }
        public int ExaminationParameterId { get; set; }
        public Guid ExaminationId { get; set; }
    }
}
