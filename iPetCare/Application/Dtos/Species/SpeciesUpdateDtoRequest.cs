using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Application.Dtos.Species
{
    public class SpeciesUpdateDtoRequest
    {
        [Required]
        public string Name { get; set; }
    }
}
