using System;
using System.Collections.Generic;
using System.Text;
using Application.Dtos.Races;

namespace Application.Dtos.Examinations
{
    public class ExaminationsGetExaminationDtoResponse
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public int ExaminationTypeId { get; set; }
        public Guid? NoteId { get; set; }
        public Guid PetId { get; set; }
        public List<ExaminationParameteterValueDetailsGetDtoResponse> ParameterValues { get; set; }
    }

    public class ExaminationParameteterValueDetailsGetDtoResponse
    {
        public Guid Id { get; set; }
        public string Value { get; set; }
        public int ExaminationParameterId { get; set; }
    }
}
