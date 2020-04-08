using System;

namespace Application.Dtos.ImportantDates
{
    public class UpdateImportantDateDtoResponse
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public Guid PetId { get; set; }
        public Guid? NoteId { get; set; }
    }
}
