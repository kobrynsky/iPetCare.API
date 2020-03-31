using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Institutions
{
    public class InstitutionsUpdateInstitutionDtoRequest
    {
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        [Required]
        [MaxLength(255)]
        public string Address { get; set; }

    }
}
