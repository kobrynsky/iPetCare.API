using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Races
{
    public class RaceCreateDtoRequest
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public int SpeciesId { get; set; }
    }
}
