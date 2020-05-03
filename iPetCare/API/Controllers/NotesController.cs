using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Application.Interfaces;
using Application.Dtos.Notes;
using Application.Services.Utilities;
using Domain.Models;
using API.Security;

namespace API.Controllers
{
    public class NotesController : BaseController
    {

        private readonly INoteService _notesService;
        public NotesController(INoteService notesService)
        {
            _notesService = notesService;
        }

        [Produces(typeof(ServiceResponse<CreateNoteDtoResponse>))]
        [AuthorizeRoles(Role.Administrator, Role.Vet, Role.Owner)]
        [HttpPost]
        public async Task<IActionResult> CreateNote(CreateNoteDtoRequest dto)
        {
            var response = await _notesService.CreateNoteAsync(dto);
            return SendResponse(response);
        }

        [Produces(typeof(ServiceResponse<GetAllNotesDtoResponse>))]
        [AuthorizeRoles(Role.Administrator)]
        [HttpGet]
        public async Task<IActionResult> GetAllNotes()
        {
            var response = await _notesService.GetAllNotesAsync();
            return SendResponse(response);
        }

        [Produces(typeof(ServiceResponse<GetImportantDatesDtoResponse>))]
        [AuthorizeRoles(Role.Owner)]
        [HttpGet("important-dates")]
        public async Task<IActionResult> GetImportantDates()
        {
            var response = await _notesService.GetImportantDates();
            return SendResponse(response);
        }

        [Produces(typeof(ServiceResponse<GetAllNotesDtoResponse>))]
        [AuthorizeRoles(Role.Administrator, Role.Vet, Role.Owner)]
        [HttpGet("{petId}")]
        public async Task<IActionResult> GetPetNotes(Guid petId)
        {
            var response = await _notesService.GetPetNotesAsync(petId);
            return SendResponse(response);
        }
        [Produces(typeof(ServiceResponse<GetAllNotesDtoResponse>))]
        [AuthorizeRoles(Role.Administrator, Role.Vet, Role.Owner)]
        [HttpGet("{petId}/{noteId}")]
        public async Task<IActionResult> GetNote(Guid petId, Guid noteId)
        {
            var response = await _notesService.GetNoteAsync(petId, noteId);
            return SendResponse(response);
        }

        [Produces(typeof(ServiceResponse<CreateNoteDtoResponse>))]
        [AuthorizeRoles(Role.Administrator, Role.Vet, Role.Owner)]
        [HttpPut("{petId}/{noteId}")]
        public async Task<IActionResult> UpdateNote(Guid petId, Guid noteId, UpdateNoteDtoRequest dto)
        {
            var response = await _notesService.UpdateNoteAsync(petId, noteId, dto);
            return SendResponse(response);
        }

        [Produces(typeof(ServiceResponse))]
        [AuthorizeRoles(Role.Administrator, Role.Vet, Role.Owner)]
        [HttpDelete("{petId}/{noteId}")]
        public async Task<IActionResult> DeleteNote(Guid petId, Guid noteId)
        {
            var response = await _notesService.DeleteNoteAsync(petId, noteId);
            return SendResponse(response);
        }
    }
}
