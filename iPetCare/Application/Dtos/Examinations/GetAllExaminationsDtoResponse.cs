using System;
using System.Collections.Generic;

namespace Application.Dtos.Examinations
{
    public class GetAllExaminationsDtoResponse
    {
        public List<ExaminationForGetAllExaminationsDtoResponse> Examinations { get; set; }
    }

    public class ExaminationForGetAllExaminationsDtoResponse
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public int ExaminationTypeId { get; set; }
        public Guid? NoteId { get; set; }
        public Guid PetId { get; set; }
    }
}
