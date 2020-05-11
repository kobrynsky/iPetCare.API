using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Races
{
    public class UpdateRaceDtoRequest
    {
        [MaxLength(255, ErrorMessage = "Długość nie może być większa, niż 255 znaków")]
        public string Name { get; set; }
        public int SpeciesId { get; set; }
    }
}
