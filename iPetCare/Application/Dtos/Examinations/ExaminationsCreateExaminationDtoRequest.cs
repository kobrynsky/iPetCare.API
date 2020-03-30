using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Examinations
{
    public class ExaminationsCreateExaminationDtoRequest
    {
        [Required]
        public DateTime Date { get; set; }
        public int ExaminationTypeId { get; set; }
        public Guid? NoteId { get; set; }
        [Required]
        public string PetId { get; set; }
    }
}
