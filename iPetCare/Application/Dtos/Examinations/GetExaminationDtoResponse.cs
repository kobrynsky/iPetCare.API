using System;
using System.Collections.Generic;

namespace Application.Dtos.Examinations
{
    public class GetExaminationDtoResponse
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public ExaminationTypeForGetExaminationDtoResponse ExaminationType { get; set; }
        public string Content { get; set; }
        public PetForGetExaminationDtoResponse Pet { get; set; }
        public List<ExaminationParameterValueForGetExaminationDtoResponse> ExaminationParameterValues { get; set; }
    }

    public class ExaminationParameterValueForGetExaminationDtoResponse
    {
        public Guid Id { get; set; }
        public float Value { get; set; }
        public ExaminationParameterForGetExaminationDtoResponse ExaminationParameter { get; set; }
    }

    public class ExaminationTypeForGetExaminationDtoResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class ExaminationParameterForGetExaminationDtoResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public float UpperLimit { get; set; }

        public float LowerLimit { get; set; }
    }

    public class PetForGetExaminationDtoResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
