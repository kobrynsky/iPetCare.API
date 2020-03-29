using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Dtos.Examinations
{
    public class ExaminationsGetAllExaminationsDtoResponse
    {
        public List<ExaminationsDetailGetAllDtoResponse> Examinations { get; set; }
    }

    public class ExaminationsDetailGetAllDtoResponse
    {
        public string Id { get; set; }
        public DateTime Date { get; set; }
        public int ExaminationTypeId { get; set; }
        public string NoteId { get; set; }
        public string PetId { get; set; }
    }
}
