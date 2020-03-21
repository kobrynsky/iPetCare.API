using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Species
{
    public class SpeciesCreateDtoRequest
    {
        [Required]
        public string Name { get; set; }
    }
}
