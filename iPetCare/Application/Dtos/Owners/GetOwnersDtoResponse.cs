using System;
using System.Collections.Generic;
using Persistence.Enums;

namespace Application.Dtos.Owners
{
    public class GetOwnersDtoResponse
    {
        public string Query { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
        public GetOwnersSortBy SortBy { get; set; }
        public List<OwnerForGetOwnersDto> Owners { get; set; }
        public string CurrentSearchingUserRole { get; set; }
    }

    public class OwnerForGetOwnersDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string PlaceOfResidence { get; set; }
        public string UserId { get; set; }
    }
}
