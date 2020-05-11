using System;
using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Examinations
{
    public class UpdateExaminationDtoRequest
    {
        public DateTime Date { get; set; }
        public int ExaminationTypeId { get; set; }

        [MaxLength(255, ErrorMessage = "Długość nie może być większa, niż 255 znaków")]
        public string Content { get; set; }

        [Required(ErrorMessage = "Pole jest wymagane")]
        public Guid PetId { get; set; }
    }
}
