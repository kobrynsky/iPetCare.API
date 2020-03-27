using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Dtos.ExaminationTypes
{
    public class ExaminationTypeUpdateDtoResponse
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public int SpeciesId { get; set; }
    }
}
