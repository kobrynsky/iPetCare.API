using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Dtos.Races
{
    public class RaceGetAllDtoResponse
    {
        public List<RaceDetailGetAllDtoResponse> Races { get; set; }
    }
    public class RaceDetailGetAllDtoResponse
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public int SpeciesId { get; set; }
    }
}
