using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Races
{
    public class CreateRaceDtoRequest
    {
        [Required(ErrorMessage = "Pole jest wymagane")]
        [MaxLength(255, ErrorMessage = "Długość nie może być większa, niż 255 znaków")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Pole jest wymagane")]
        public int SpeciesId { get; set; }
    }
}
