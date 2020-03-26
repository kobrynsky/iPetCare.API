using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Dtos.Races
{
    public class RaceGetDtoResponse
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public SpeciesDetailsGetDtoResponse Species { get; set; }
    }
    public class SpeciesDetailsGetDtoResponse
    {
        public string Name { get; set; }
        public int Id { get; set; }
    }
}
