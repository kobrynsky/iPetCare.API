using System;
using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.ImportantDates
{
    public class CreateImportatnDateDtoRequest
    {
        [Required]
        [MaxLength(255)]
        public string Title { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public Guid NoteId { get; set; }
    }
}
