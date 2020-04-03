using System;
using System.Collections.Generic;

namespace Application.Dtos.Institutions
{
    public class GetInstitutionsDtoResponse
    {
        public ICollection<InstitutionForGetInstitutionDtoResponse> Institutions { get; set; }
    }

    public class InstitutionForGetInstitutionDtoResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
    }
}
