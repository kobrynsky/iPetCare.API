using System;
using System.Linq;
using System.Net;
using System.Collections.Generic;
using Application.Interfaces;
using Application.Services.Utilities;
using Application.Dtos.ImportantDates;
using Domain.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    public class ImportantDatesService : Service, IImportantDatesService
    {
        public ImportantDatesService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public async Task<ServiceResponse<CreateImportantDateDtoResponse>> CreateImportantDateAsync(CreateImportantDateDtoRequest dto)
        {
            if (CurrentlyLoggedUser == null)
                return new ServiceResponse<CreateImportantDateDtoResponse>(HttpStatusCode.Unauthorized);

            var pet = Context.Pets.Find(dto.PetId);

            if (pet == null)
                return new ServiceResponse<CreateImportantDateDtoResponse>(HttpStatusCode.BadRequest, "Nie znaleziono zwierzaka");

            if (!CanEditImportantDate(pet))
                return new ServiceResponse<CreateImportantDateDtoResponse>(HttpStatusCode.Forbidden);

            if (dto.NoteId != null)
            {
                var note = Context.Notes.Find(dto.NoteId);
                if (note == null)
                    return new ServiceResponse<CreateImportantDateDtoResponse>(HttpStatusCode.BadRequest, "Nie istnieje taka notatka w bazie danych");
            }

            ImportantDate importantDate = Mapper.Map<ImportantDate>(dto);

             var importantDatePet = new ImportantDatePet()
             {
                 ImportantDateId = importantDate.Id,
                 PetId = dto.PetId
             };

            Context.ImportantDatePets.Add(importantDatePet);
            Context.ImportantDates.Add(importantDate);
            int result = await Context.SaveChangesAsync();

            var responseDto = Mapper.Map<CreateImportantDateDtoResponse>(importantDate);

            return result > 0
                ? new ServiceResponse<CreateImportantDateDtoResponse>(HttpStatusCode.OK, responseDto)
                : new ServiceResponse<CreateImportantDateDtoResponse>(HttpStatusCode.BadRequest, "Nie nastąpiło zapisanie do bazy danych");
        }

        public async Task<ServiceResponse> DeleteImportantDateAsync(Guid importantDateId)
        {
            if (CurrentlyLoggedUser == null)
                return new ServiceResponse(HttpStatusCode.Unauthorized);

            var importantDatePet = Context.ImportantDatePets.Find(importantDateId);
            if (importantDatePet == null)
                return new ServiceResponse(HttpStatusCode.NotFound);

            var pet = Context.Pets.Find(importantDatePet.PetId);

            if (pet == null)
                return new ServiceResponse(HttpStatusCode.BadRequest, "Nie znaleziono zwierzaka");

            if (!CanEditImportantDate(pet))
                return new ServiceResponse(HttpStatusCode.Forbidden);

            var importantDate = Context.ImportantDates.Find(importantDateId);
            if (importantDate == null)
                return new ServiceResponse(HttpStatusCode.NotFound);

            Context.ImportantDates.Remove(importantDate);
            int result = await Context.SaveChangesAsync();

            return result > 0
                ? new ServiceResponse(HttpStatusCode.OK)
                : new ServiceResponse(HttpStatusCode.BadRequest, "Wystąpił błąd podczas usuwania badania");
        }

        public async Task<ServiceResponse<GetAllImportantDatesDtoResponse>> GetAllImportantDatesAsync()
        {
            if (CurrentlyLoggedUser == null)
                return new ServiceResponse<GetAllImportantDatesDtoResponse>(HttpStatusCode.Unauthorized);

            var importantDates = await Context.ImportantDates.ToListAsync();

            var dto = new GetAllImportantDatesDtoResponse()
            {
                ImportantDates = Mapper.Map<List<ImportantDateForGetAllImportantDatesDtoResponse>>(importantDates)
            };

            return new ServiceResponse<GetAllImportantDatesDtoResponse>(HttpStatusCode.OK, dto);
        }

        public async Task<ServiceResponse<GetImportantDateDtoResponse>> GetImportantDateAsync(Guid importantDateId)
        {
            if (CurrentlyLoggedUser == null)
                return new ServiceResponse<GetImportantDateDtoResponse>(HttpStatusCode.Unauthorized);

            var importantDatePet = Context.ImportantDatePets.Find(importantDateId);
            if (importantDatePet == null)
                return new ServiceResponse<GetImportantDateDtoResponse>(HttpStatusCode.NotFound);

            var pet = Context.Pets.Find(importantDatePet.PetId);

            if (pet == null)
                return new ServiceResponse<GetImportantDateDtoResponse>(HttpStatusCode.BadRequest, "Nie znaleziono zwierzaka");

            if (!CanEditImportantDate(pet))
                return new ServiceResponse<GetImportantDateDtoResponse>(HttpStatusCode.Forbidden);

            var importantDate = await Context.ImportantDates.FindAsync(importantDateId);
            if (importantDate == null)
                return new ServiceResponse<GetImportantDateDtoResponse>(HttpStatusCode.NotFound);

            var dto = Mapper.Map<GetImportantDateDtoResponse>(importantDate);

            var note = Context.Notes.Find(importantDate.NoteId);

            if (note != null)
                dto.Note = Mapper.Map<NoteForGetImportantDateDtoResponse>(note);

            return new ServiceResponse<GetImportantDateDtoResponse>(HttpStatusCode.OK, dto);
        }

        public async Task<ServiceResponse<UpdateImportantDateDtoResponse>> UpdateImportantDateAsync(Guid importantDateId, UpdateImportantDateDtoRequest dto)
        {
            if (CurrentlyLoggedUser == null)
                return new ServiceResponse<UpdateImportantDateDtoResponse>(HttpStatusCode.Unauthorized);

            var importantDatePet = Context.ImportantDatePets.Find(importantDateId);
            if (importantDatePet == null)
                return new ServiceResponse<UpdateImportantDateDtoResponse>(HttpStatusCode.NotFound);

            var pet = Context.Pets.Find(importantDatePet.PetId);
            if (pet == null)
                return new ServiceResponse<UpdateImportantDateDtoResponse>(HttpStatusCode.BadRequest, "Nie znaleziono zwierzaka");

            if (!CanEditImportantDate(pet))
                return new ServiceResponse<UpdateImportantDateDtoResponse>(HttpStatusCode.Forbidden);

            var importantDate = Context.ImportantDates.Find(importantDateId);
            Note note = null;

            if (dto.NoteId != null)
            {
                note = Context.Notes.Find(dto.NoteId);
                if (note == null)
                    return new ServiceResponse<UpdateImportantDateDtoResponse>(HttpStatusCode.BadRequest, "Nie istnieje taka notatka w bazie danych");
            }

            if (importantDate == null)
                return new ServiceResponse<UpdateImportantDateDtoResponse>(HttpStatusCode.NotFound);

            if (note == null)
                importantDate.NoteId = null;
            else
                importantDate.NoteId = dto.NoteId;

            importantDate.Date = dto.Date;
            importantDate.Title = dto.Title;
            importantDatePet.PetId = dto.PetId;

            int result = await Context.SaveChangesAsync();
            if (result > 0)
            {
                var responseDto = Mapper.Map<UpdateImportantDateDtoResponse>(importantDate);
                return new ServiceResponse<UpdateImportantDateDtoResponse>(HttpStatusCode.OK, responseDto);
            }

            return new ServiceResponse<UpdateImportantDateDtoResponse>(HttpStatusCode.BadRequest, "Wystąpił błąd podczas zapisu badania");
        }

        private bool CanEditImportantDate(Pet pet)
        {
            if (CurrentlyLoggedUser.Role != Role.Administrator)
            {
                if (CurrentlyLoggedUser.Role == Role.Owner)
                {
                    var owners = pet.OwnerPets.Where(ownerpet => ownerpet.OwnerId == CurrentlyLoggedUser.Owner.Id);
                    if (!owners.Any())
                        return false;
                }
                if (CurrentlyLoggedUser.Role == Role.Vet)
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
