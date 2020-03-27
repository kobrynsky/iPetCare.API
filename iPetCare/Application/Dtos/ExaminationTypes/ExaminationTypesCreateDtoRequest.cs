using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.ExaminationTypes
{
    public class ExaminationTypesCreateDtoRequest
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public int SpeciesId { get; set; }
    }
}
