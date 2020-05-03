using Application.Dtos.Invitations;
using Persistence.Enums;

namespace Application.Dtos.Vets
{
    public class GetVetsDtoRequest
    {
        public string Query { get; set; }
        public int? Page { get; set; }
        public int? PageSize { get; set; }
        public GetVetsSortBy? SortBy { get; set; }
    }
}
