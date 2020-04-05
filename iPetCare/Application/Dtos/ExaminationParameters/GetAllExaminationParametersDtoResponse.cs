using System.Collections.Generic;

namespace Application.Dtos.ExaminationParameters
{
    public class GetAllExaminationParametersDtoResponse
    {
        public List<ExaminationParameterForGetAllExaminationParametersDtoResponse> ExaminationParameters { get; set; }
    }

    public class ExaminationParameterForGetAllExaminationParametersDtoResponse
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public float UpperLimit { get; set; }
        public float LowerLimit { get; set; }
        public int ExaminationTypeId { get; set; }
    }
}
