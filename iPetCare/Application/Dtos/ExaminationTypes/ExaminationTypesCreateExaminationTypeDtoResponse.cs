using System;
using System.Collections.Generic;

namespace Application.Dtos.ExaminationTypes
{
    public class ExaminationTypesCreateExaminationTypeDtoResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int SpeciesId { get; set; }
    }
}
