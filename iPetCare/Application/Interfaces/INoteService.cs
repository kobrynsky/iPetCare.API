using System;
using System.Threading.Tasks;
using Application.Dtos.Notes;
using Application.Services.Utilities;

namespace Application.Interfaces
{
    public interface INoteService
    {
        Task<ServiceResponse<CreateNoteDtoResponse>> CreateNoteAsync(CreateNoteDtoRequest dto);
        Task<ServiceResponse<GetAllNotesDtoResponse>> GetAllNotesAsync();
        Task<ServiceResponse<GetNoteDtoResponse>> GetNoteAsync(Guid petId, Guid noteId);
        Task<ServiceResponse<UpdateNoteDtoResponse>> UpdateNoteAsync(Guid petId, Guid noteId, UpdateNoteDtoRequest dto);
        Task<ServiceResponse> DeleteNoteAsync(Guid petId, Guid noteId);
        Task<ServiceResponse<GetAllNotesDtoResponse>> GetPetNotesAsync(Guid petId);
        Task<ServiceResponse<GetImportantDatesDtoResponse>> GetImportantDates();
    }
}
