using System;
using System.Collections.Generic;

namespace Application.Dtos.Institutions
{
    public class InstitutionsGetInstitutionsDtoResponse
    {
        public ICollection<InstitutionForInstitutionGetInstitutionDtoResponse> Institutions { get; set; }
    }

    public class InstitutionForInstitutionGetInstitutionDtoResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
    }
}
