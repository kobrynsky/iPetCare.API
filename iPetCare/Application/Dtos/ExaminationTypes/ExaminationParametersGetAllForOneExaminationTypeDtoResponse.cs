using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Dtos.ExaminationTypes
{
    public class ExaminationParametersGetAllForOneExaminationTypeDtoResponse
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public List<ExaminationParameterDetailsForExaminationTypeGetDtoResponse> ExaminationParameters { get; set; }
    }

    public class ExaminationParameterDetailsForExaminationTypeGetDtoResponse
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public float UpperLimit { get; set; }
        public float LowerLimit { get; set; }
        public int ExaminationTypeId { get; set; }
    }
}
