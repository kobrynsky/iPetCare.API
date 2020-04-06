using System;
using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.ImportantDates
{
    public class UpdateImportatnDateDtoRequest
    {
        [MaxLength(255)]
        public string Title { get; set; }

        public DateTime Date { get; set; }

        public Guid NoteId { get; set; }
    }
}
