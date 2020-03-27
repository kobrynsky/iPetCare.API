using System;
using System.Collections.Generic;
using System.Text;
using Application.Dtos.Races;

namespace Application.Dtos.ExaminationTypes
{
    public class ExaminationTypeGetDtoResponse
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public SpeciesDetailsGetDtoResponse Species { get; set; }
    }
}
