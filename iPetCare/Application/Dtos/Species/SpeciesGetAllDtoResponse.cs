using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Dtos.Species
{
    public class SpeciesGetAllDtoResponse
    {
        public List<SpeciesDetailGetAllDtoResponse> Species{ get; set; }
    }
    public class SpeciesDetailGetAllDtoResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
