using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Dtos.Species
{
    public class SpeciesGetDtoResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<RaceDetailsGetDtoResponse> Races { get; set; }
    }
    public class RaceDetailsGetDtoResponse
    {
        public string Name { get; set; }
        public int Id { get; set; }
    }
}
