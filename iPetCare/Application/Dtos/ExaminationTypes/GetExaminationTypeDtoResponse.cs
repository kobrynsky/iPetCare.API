using Application.Dtos.Races;

namespace Application.Dtos.ExaminationTypes
{
    public class GetExaminationTypeDtoResponse
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public SpeciesDetailsGetDtoResponse Species { get; set; }
    }
}
