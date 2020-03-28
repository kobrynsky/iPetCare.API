using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Dtos.ExaminationTypes
{
    public class ExaminationTypesUpdateExaminationTypeDtoRequest
    {
        public string Name { get; set; }
        public int SpeciesId { get; set; }
    }
}
