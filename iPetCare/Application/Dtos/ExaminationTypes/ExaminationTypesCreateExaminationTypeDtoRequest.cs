using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.ExaminationTypes
{
    public class ExaminationTypesCreateExaminationTypeDtoRequest
    {
        [Required]
        public string Name { get; set; }
        public int SpeciesId { get; set; }
    }
}
