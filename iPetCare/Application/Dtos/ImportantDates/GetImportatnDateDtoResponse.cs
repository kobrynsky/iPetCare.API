using Domain.Models;
using System;

namespace Application.Dtos.ImportantDates
{
    public class GetImportatnDateDtoResponse
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public Note Note { get; set; }
    }

    public class NoteForGetExaminationParameterDtoResponse
    {

        public Guid Id { get; set; }

        public string Payload { get; set; }

        public DateTime CreatedAt { get; set; }

        public string UserId { get; set; }
    }
}
