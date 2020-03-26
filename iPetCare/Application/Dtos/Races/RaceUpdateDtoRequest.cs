using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Dtos.Races
{
    public class RaceUpdateDtoRequest
    {
        public string Name { get; set; }
        public int SpeciesId { get; set; }
    }
}
