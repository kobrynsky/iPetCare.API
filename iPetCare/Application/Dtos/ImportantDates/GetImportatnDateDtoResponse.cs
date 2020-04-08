using Domain.Models;
using System;

namespace Application.Dtos.ImportantDates
{
    public class GetImportantDateDtoResponse
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public Guid PetId { get; set; }
        public NoteForGetImportantDateDtoResponse? Note { get; set; }
    }

    public class NoteForGetImportantDateDtoResponse
    {

        public Guid Id { get; set; }

        public string Payload { get; set; }

        public DateTime CreatedAt { get; set; }

        public string UserId { get; set; }
    }
}
