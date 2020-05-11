using System.ComponentModel.DataAnnotations;
using Persistence.Enums;

namespace Application.Dtos.Vets
{
    public class GetVetsDtoRequest
    {
        [MaxLength(255, ErrorMessage = "Długość nie może być większa, niż 255 znaków")]
        public string Query { get; set; }
        public int? Page { get; set; }
        public int? PageSize { get; set; }
        public GetVetsSortBy? SortBy { get; set; }
    }
}
