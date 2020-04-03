using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.ExaminationTypes
{
    public class CreateExaminationTypeDtoRequest
    {
        [Required]
        public string Name { get; set; }
        public int SpeciesId { get; set; }
    }
}
