using System;
using System.Collections.Generic;

namespace Application.Dtos.Institutions
{
    public class GetInstitutionDtoResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public ICollection<UserForGetInstitutionDtoResponse> Vets { get; set; }
    }

    public class UserForGetInstitutionDtoResponse
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
    }
}
