using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Dtos.ExaminationParameterValues
{
    public class ExaminationParameterValuesGetExaminationParameterValueDtoResponse
    {
        public Guid Id { get; set; }
        public float Value { get; set; }

        public ExaminationDetailsGetDtoResponse Examination { get; set; }
        public ExaminationParameterDetailsGetDtoResponse ExaminationParameter { get; set; }
    }

    public class ExaminationDetailsGetDtoResponse
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public int ExaminationTypeId { get; set; }
        public Guid? NoteId { get; set; }
        public Guid PetId { get; set; }
    }

    public class ExaminationParameterDetailsGetDtoResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public float UpperLimit { get; set; }
        public float LowerLimit { get; set; }
        public int ExaminationTypeId { get; set; }
    }
}
