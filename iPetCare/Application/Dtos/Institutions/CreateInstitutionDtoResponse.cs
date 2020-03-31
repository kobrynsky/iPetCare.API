using System;

namespace Application.Dtos.Institutions
{
    public class CreateInstitutionDtoResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
    }
}
