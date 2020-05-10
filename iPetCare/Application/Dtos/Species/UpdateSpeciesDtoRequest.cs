using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Species
{
    public class UpdateSpeciesDtoRequest
    {
        [Required(ErrorMessage = "Pole jest wymagane")]
        [MaxLength(255, ErrorMessage = "Długość nie może być większa, niż 255 znaków")]
        public string Name { get; set; }
    }
}
