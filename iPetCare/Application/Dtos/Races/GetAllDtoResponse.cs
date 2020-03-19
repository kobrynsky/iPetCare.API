using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Dtos.Races
{
    public class GetAllDtoResponse
    {
        public List<RaceGetAllDtoResponse> Races { get; set; }
    }
    public class RaceGetAllDtoResponse
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public int SpeciesId { get; set; }
    }
}
