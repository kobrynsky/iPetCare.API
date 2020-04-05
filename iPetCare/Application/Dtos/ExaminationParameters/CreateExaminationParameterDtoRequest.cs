using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.ExaminationParameters
{
    public class CreateExaminationParameterDtoRequest
    {
        [Required]
        public string Name { get; set; }

        [Range(0, float.MaxValue, ErrorMessage = "Please enter valid float Number")]
        public float UpperLimit { get; set; }

        [Range(0, float.MaxValue, ErrorMessage = "Please enter valid float Number")]
        public float LowerLimit { get; set; }
        [Required]
        public int ExaminationTypeId { get; set; }
    }
}
