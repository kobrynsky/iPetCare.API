using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.ExaminationParameters
{
    public class UpdateExaminationParameterDtoRequest
    {
        [MaxLength(255, ErrorMessage = "Długość nie może być większa, niż 255 znaków")]
        public string Name { get; set; }
        public float UpperLimit { get; set; }
        public float LowerLimit { get; set; }
    }
}
