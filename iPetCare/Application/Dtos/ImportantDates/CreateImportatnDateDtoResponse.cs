using System;

namespace Application.Dtos.ImportantDates
{
    public class CreateImportatnDateDtoResponse
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public Guid NoteId { get; set; }
    }
}
