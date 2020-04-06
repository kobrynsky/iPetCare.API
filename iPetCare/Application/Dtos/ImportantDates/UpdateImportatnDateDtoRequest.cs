using System;
using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.ImportantDates
{
    public class UpdateImportantDateDtoRequest
    {
        [MaxLength(255)]
        public string Title { get; set; }

        public DateTime Date { get; set; }
        public Guid PetId { get; set; }
        public Guid? NoteId { get; set; }
    }
}
