using Persistence.Enums;

namespace Application.Dtos.Owners
{
    public class GetOwnersDtoRequest
    {
        public string Query { get; set; }
        public int? Page { get; set; }
        public int? PageSize { get; set; }
        public GetOwnersSortBy? SortBy { get; set; }
    }
}
