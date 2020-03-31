using System.Collections.Generic;

namespace Application.Dtos.Races
{
    public class GetAllRacesDtoResponse
    {
        public List<RaceForGetAllRacesDtoResponse> Races { get; set; }
    }
    public class RaceForGetAllRacesDtoResponse
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public int SpeciesId { get; set; }
    }
}
