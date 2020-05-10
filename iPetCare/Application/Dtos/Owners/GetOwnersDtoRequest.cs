using System.ComponentModel.DataAnnotations;
using Persistence.Enums;

namespace Application.Dtos.Owners
{
    public class GetOwnersDtoRequest
    {
        [MaxLength(255, ErrorMessage = "Długość nie może być większa, niż 255 znaków")]
        public string Query { get; set; }
        public int? Page { get; set; }
        public int? PageSize { get; set; }
        public GetOwnersSortBy? SortBy { get; set; }
    }
}
