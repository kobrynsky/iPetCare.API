using System.Collections.Generic;

namespace Application.Dtos.Species
{
    public class GetSpeciesDtoResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<RaceForGetSpeciesDtoResponse> Races { get; set; }
    }
    public class RaceForGetSpeciesDtoResponse
    {
        public string Name { get; set; }
        public int Id { get; set; }
    }
}
