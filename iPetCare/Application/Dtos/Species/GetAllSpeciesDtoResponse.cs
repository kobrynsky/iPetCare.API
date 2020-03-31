using System.Collections.Generic;

namespace Application.Dtos.Species
{
    public class GetAllSpeciesDtoResponse
    {
        public List<SpeciesForGetAllSpeciesDtoResponse> Species{ get; set; }
    }
    public class SpeciesForGetAllSpeciesDtoResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
