using System;
using System.Collections.Generic;

namespace Application.Dtos.ImportantDates
{
    public class GetAllImportatnDatesDtoResponse
    {
        public List<ImportantDateForGetAllImportantDatesDtoResponse> ImportantDates { get; set; }
    }

    public class ImportantDateForGetAllImportantDatesDtoResponse
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public Guid NoteId { get; set; }
    }
}
