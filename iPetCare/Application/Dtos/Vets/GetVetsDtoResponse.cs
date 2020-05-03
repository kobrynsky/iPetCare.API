using System;
using System.Collections.Generic;
using Persistence.Enums;

namespace Application.Dtos.Vets
{
    public class GetVetsDtoResponse
    {
        public string Query { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
        public GetVetsSortBy SortBy { get; set; }

        public List<VetForGetVetsDto> Vets { get; set; }
        public string CurrentSearchingUserRole { get; set; }
    }

    public class VetForGetVetsDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string Specialization { get; set; }
        public Guid Id { get; set; }
        public List<InstitutionForGetVetsDto> Institutions { get; set; }
    }

    public class InstitutionForGetVetsDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
    }
}
