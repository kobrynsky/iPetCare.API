using System;
using System.Collections.Generic;

namespace Application.Dtos.Notes
{
    public class GetImportantDatesDtoResponse
    {
        public List<NoteForGetImportantDatesDtoResponse> UpcomingDates { get; set; }   
        public List<NoteForGetImportantDatesDtoResponse> PastDates { get; set; }
    }

    public class NoteForGetImportantDatesDtoResponse
    {
        public Guid Id { get; set; }
        public string Payload { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ImportantDate { get; set; }
        public Guid PetId { get; set; }
    }
}
