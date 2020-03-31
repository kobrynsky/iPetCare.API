namespace Application.Dtos.Races
{
    public class GetRaceDtoResponse
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
