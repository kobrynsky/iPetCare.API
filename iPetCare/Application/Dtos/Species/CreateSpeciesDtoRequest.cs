using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Species
{
    public class CreateSpeciesDtoRequest
    {
        [Required(ErrorMessage = "Pole jest wymagane")]
        [MaxLength(50, ErrorMessage = "Długość nie może być większa, niż 50 znaków")]
        public string Name { get; set; }
    }
}
