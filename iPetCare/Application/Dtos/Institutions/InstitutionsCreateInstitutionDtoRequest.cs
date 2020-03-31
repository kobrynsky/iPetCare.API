using System;
using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Institutions
{
    public class InstitutionsCreateInstitutionDtoRequest
    {
        public Guid Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        [Required]
        [MaxLength(255)]
        public string Address { get; set; }
    }
}
