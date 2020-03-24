using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Species
{
    public class SpeciesCreateSpeciesDtoRequest
    {
        [Required][MaxLength(50)]
        public string Name { get; set; }
    }
}
