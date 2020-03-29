using System;
using System.Collections.Generic;
using System.Text;
using Application.Dtos.Races;

namespace Application.Dtos.Examinations
{
    public class ExaminationsGetExaminationDtoResponse
    {
        public string Id { get; set; }
        public DateTime Date { get; set; }
        public int ExaminationTypeId { get; set; }
        public string NoteId { get; set; }
        public string PetId { get; set; }
        public List<ExaminationParameteterValueDetailsGetDtoResponse> ParameterValues { get; set; }
    }

    public class ExaminationParameteterValueDetailsGetDtoResponse
    {
        public string Id { get; set; }
        public string Value { get; set; }
        public int ExaminationParameterId { get; set; }
    }
}
