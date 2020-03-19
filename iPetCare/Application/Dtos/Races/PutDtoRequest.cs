using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Dtos.Races
{
    public class PutDtoRequest
    {
        public string Name { get; set; }
        public int SpeciesId { get; set; }
    }
}
