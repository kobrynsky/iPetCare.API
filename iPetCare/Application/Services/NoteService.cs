using System;
using System.Linq;
using System.Collections.Generic;
using System.Net;
using Application.Interfaces;
using Application.Services.Utilities;
using Application.Dtos.Notes;
using Domain.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    public class NoteService : Service, INoteService
    {
        public NoteService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public async Task<ServiceResponse<CreateNoteDtoResponse>> CreateNoteAsync(CreateNoteDtoRequest dto)
        {
            if (CurrentlyLoggedUser == null)
                return new ServiceResponse<CreateNoteDtoResponse>(HttpStatusCode.Unauthorized);

            var pet = await Context.Pets.FindAsync(dto.PetId);

            if (pet == null)
                return new ServiceResponse<CreateNoteDtoResponse>(HttpStatusCode.BadRequest, "Nie znaleziono zwierzaka");

            if (!CanEditNote(pet))
                return new ServiceResponse<CreateNoteDtoResponse>(HttpStatusCode.Forbidden);

            Note note = Mapper.Map<Note>(dto);
            note.UserId = CurrentlyLoggedUser.Id;
            Context.Notes.Add(note);
            int result = await Context.SaveChangesAsync();

            var responseDto = Mapper.Map<CreateNoteDtoResponse>(note);

            return result > 0
                ? new ServiceResponse<CreateNoteDtoResponse>(HttpStatusCode.OK, responseDto)
                : new ServiceResponse<CreateNoteDtoResponse>(HttpStatusCode.BadRequest, "Nie nastąpiło zapisanie do bazy danych");
        }

        public async Task<ServiceResponse> DeleteNoteAsync(Guid petId, Guid noteId)
        {
            if (CurrentlyLoggedUser == null)
                return new ServiceResponse(HttpStatusCode.Unauthorized);

            var pet = await Context.Pets.FindAsync(petId);

            if (pet == null)
                return new ServiceResponse(HttpStatusCode.BadRequest, "Nie znaleziono zwierzaka");

            if (!CanEditNote(pet))
                return new ServiceResponse(HttpStatusCode.Forbidden);

            var note = await Context.Notes.FindAsync(noteId);
            if (note == null)
                return new ServiceResponse(HttpStatusCode.NotFound);

            Context.Notes.Remove(note);
            int result = await Context.SaveChangesAsync();

            return result > 0
                ? new ServiceResponse(HttpStatusCode.OK)
                : new ServiceResponse(HttpStatusCode.BadRequest, "Wystąpił błąd podczas usuwania badania");
        }

        public async Task<ServiceResponse<GetAllNotesDtoResponse>> GetAllNotesAsync()
        {
            if (CurrentlyLoggedUser == null)
                return new ServiceResponse<GetAllNotesDtoResponse>(HttpStatusCode.Unauthorized);

            var notes = await Context.Notes.ToListAsync();

            var dto = new GetAllNotesDtoResponse()
            {
                Notes = Mapper.Map<List<NoteForGetAllNotesDtoResponse>>(notes)
            };

            return new ServiceResponse<GetAllNotesDtoResponse>(HttpStatusCode.OK, dto);
        }

        public async Task<ServiceResponse<GetNoteDtoResponse>> GetNoteAsync(Guid petId, Guid noteId)
        {
            if (CurrentlyLoggedUser == null)
                return new ServiceResponse<GetNoteDtoResponse>(HttpStatusCode.Unauthorized);

            var pet = await Context.Pets.FindAsync(petId);

            if (pet == null)
                return new ServiceResponse<GetNoteDtoResponse>(HttpStatusCode.BadRequest, "Nie znaleziono zwierzaka");

            if (!CanEditNote(pet))
                return new ServiceResponse<GetNoteDtoResponse>(HttpStatusCode.Forbidden);

            var note = await Context.Notes.FindAsync(noteId);
            if (note == null)
                return new ServiceResponse<GetNoteDtoResponse>(HttpStatusCode.NotFound);

            var dto = Mapper.Map<GetNoteDtoResponse>(note);

            return new ServiceResponse<GetNoteDtoResponse>(HttpStatusCode.OK, dto);
        }

        public async Task<ServiceResponse<GetAllNotesDtoResponse>> GetPetNotesAsync(Guid petId)
        {
            if (CurrentlyLoggedUser == null)
                return new ServiceResponse<GetAllNotesDtoResponse>(HttpStatusCode.Unauthorized);

            var pet = await Context.Pets.FindAsync(petId);

            if (pet == null)
                return new ServiceResponse<GetAllNotesDtoResponse>(HttpStatusCode.NotFound);

            if (!CanEditNote(pet))
                return new ServiceResponse<GetAllNotesDtoResponse>(HttpStatusCode.Forbidden);

            var notes = await Context.Notes.Where(ex => ex.PetId == petId).ToListAsync();

            var dto = new GetAllNotesDtoResponse()
            {
                Notes = Mapper.Map<List<NoteForGetAllNotesDtoResponse>>(notes)
            };

            return new ServiceResponse<GetAllNotesDtoResponse>(HttpStatusCode.OK, dto);
        }

        public async Task<ServiceResponse<UpdateNoteDtoResponse>> UpdateNoteAsync(Guid petId, Guid noteId, UpdateNoteDtoRequest dto)
        {
            if (CurrentlyLoggedUser == null)
                return new ServiceResponse<UpdateNoteDtoResponse>(HttpStatusCode.Unauthorized);

            var pet = await Context.Pets.FindAsync(petId);
            if (pet == null)
                return new ServiceResponse<UpdateNoteDtoResponse>(HttpStatusCode.BadRequest, "Nie znaleziono zwierzaka");

            if (noteId == Guid.Empty)
                return new ServiceResponse<UpdateNoteDtoResponse>(HttpStatusCode.BadRequest, "Nieprawidłowy indeks");

            if (!CanEditNote(pet))
                return new ServiceResponse<UpdateNoteDtoResponse>(HttpStatusCode.Forbidden);

            var note = await Context.Notes.FindAsync(noteId);

            if (note == null)
                return new ServiceResponse<UpdateNoteDtoResponse>(HttpStatusCode.NotFound);

            if (note.Payload == dto.Payload && note.CreatedAt == dto.CreatedAt && note.PetId == dto.PetId && note.UserId == CurrentlyLoggedUser.Id)
            {
                var responseDto = Mapper.Map<UpdateNoteDtoResponse>(note);
                return new ServiceResponse<UpdateNoteDtoResponse>(HttpStatusCode.OK, responseDto);
            }

            note.Payload = dto.Payload;
            note.CreatedAt = dto.CreatedAt;
            note.ImportantDate = dto.ImportantDate;
            note.PetId = dto.PetId;
            note.UserId = CurrentlyLoggedUser.Id;

            int result = await Context.SaveChangesAsync();
            if (result > 0)
            {
                var responseDto = Mapper.Map<UpdateNoteDtoResponse>(note);
                return new ServiceResponse<UpdateNoteDtoResponse>(HttpStatusCode.OK, responseDto);
            }

            return new ServiceResponse<UpdateNoteDtoResponse>(HttpStatusCode.BadRequest, "Wystąpił błąd podczas zapisu badania");
        }

        private bool CanEditNote(Pet pet)
        {
            if (CurrentlyLoggedUser.Role != Role.Administrator)
            {
                if (CurrentlyLoggedUser.Role == Role.Owner)
                {
                    var owners = pet.OwnerPets.Where(ownerpet => ownerpet.OwnerId == CurrentlyLoggedUser.Owner.Id);
                    if (!owners.Any())
                        return false;
                }
                if(CurrentlyLoggedUser.Role == Role.Vet)
                {
                    var vets = pet.VetPets.Where(vetpet => vetpet.VetId == CurrentlyLoggedUser.Vet.Id);
                    if (!vets.Any())
                        return false;
                }
            }
            return true;
        }
    }
}
