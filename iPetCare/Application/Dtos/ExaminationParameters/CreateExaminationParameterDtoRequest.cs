using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.ExaminationParameters
{
    public class CreateExaminationParameterDtoRequest
    {
        [Required(ErrorMessage = "Pole jest wymagane")]
        [MaxLength(255, ErrorMessage = "Długość nie może być większa, niż 255 znaków")]
        public string Name { get; set; }

        [Range(0, float.MaxValue, ErrorMessage = "Należy wprowadzić liczbę w poprawnym formacie")]
        public float UpperLimit { get; set; }

        [Range(0, float.MaxValue, ErrorMessage = "Należy wprowadzić liczbę w poprawnym formacie")]
        public float LowerLimit { get; set; }
        [Required(ErrorMessage = "Pole jest wymagane")]
        public int ExaminationTypeId { get; set; }
    }
}
