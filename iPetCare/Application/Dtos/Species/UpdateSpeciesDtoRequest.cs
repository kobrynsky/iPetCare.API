using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Species
{
    public class UpdateSpeciesDtoRequest
    {
        [Required]
        public string Name { get; set; }
    }
}
