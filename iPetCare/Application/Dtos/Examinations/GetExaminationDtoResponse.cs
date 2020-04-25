using System;
using System.Collections.Generic;

namespace Application.Dtos.Examinations
{
    public class GetExaminationDtoResponse
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public int ExaminationTypeId { get; set; }
        public string Content { get; set; }
        public Guid PetId { get; set; }
        public List<ParameterValueForGetExaminationDtoResponse> ParameterValues { get; set; }
    }

    public class ParameterValueForGetExaminationDtoResponse
    {
        public Guid Id { get; set; }
        public float Value { get; set; }
        public int ExaminationParameterId { get; set; }
    }
}
