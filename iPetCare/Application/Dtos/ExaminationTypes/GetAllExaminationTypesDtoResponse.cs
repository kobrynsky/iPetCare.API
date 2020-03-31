using System.Collections.Generic;

namespace Application.Dtos.ExaminationTypes
{
    public class GetAllExaminationTypesDtoResponse
    {
        public List<ExaminationTypeForGetAllExaminationTypesDtoResponse> ExaminationTypes { get; set; }
    }

    public class ExaminationTypeForGetAllExaminationTypesDtoResponse
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public int SpeciesId { get; set; }
    }
}
