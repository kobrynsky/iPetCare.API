using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Dtos.ExaminationParameterValues
{
    public class GetExaminationParameterValueDtoResponse
    {
        public Guid Id { get; set; }
        public float Value { get; set; }

        public ExaminationForGetExaminationParametersValuesDtoResponse Examination { get; set; }
        public ExaminationParameterForGetExaminationParametersValuesDtoResponse ExaminationParameter { get; set; }
    }

    public class ExaminationForGetExaminationParametersValuesDtoResponse
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public int ExaminationTypeId { get; set; }
        public Guid? NoteId { get; set; }
        public Guid PetId { get; set; }
    }

    public class ExaminationParameterForGetExaminationParametersValuesDtoResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public float UpperLimit { get; set; }
        public float LowerLimit { get; set; }
        public int ExaminationTypeId { get; set; }
    }
}
