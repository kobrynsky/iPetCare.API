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
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public int ExaminationTypeId { get; set; }
        public Guid? NoteId { get; set; }
        public Guid PetId { get; set; }
    }
}
