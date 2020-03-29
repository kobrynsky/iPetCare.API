using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Dtos.ExaminationTypes
{
    public class ExaminationTypesGetAllExaminationTypesDtoResponse
    {
        public List<ExaminationTypesDetailGetAllDtoResponse> ExaminationTypes { get; set; }
    }

    public class ExaminationTypesDetailGetAllDtoResponse
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public int SpeciesId { get; set; }
    }
}
