using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Races
{
    public class CreateRaceDtoRequest
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public int SpeciesId { get; set; }
    }
}
