using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Application.Dtos.Examinations
{
    public class ExaminationsUpdateExaminationDtoRequest
    {
        public DateTime Date { get; set; }
        public int ExaminationTypeId { get; set; }
        public Guid? NoteId { get; set; }

        [Required]
        public Guid PetId { get; set; }
    }
}
