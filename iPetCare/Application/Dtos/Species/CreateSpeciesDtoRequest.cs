using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Species
{
    public class CreateSpeciesDtoRequest
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
    }
}
