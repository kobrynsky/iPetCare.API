using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Dtos.Notes
{
    public class GetAllNotesDtoResponse
    {
        public List<NoteForGetAllNotesDtoResponse> Notes { get; set; }
    }

    public class NoteForGetAllNotesDtoResponse
    {
        public Guid Id { get; set; }
        public string Payload { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid PetId { get; set; }
        public string UserId { get; set; }
    }
}
