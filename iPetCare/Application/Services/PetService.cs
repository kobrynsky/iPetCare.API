using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using Application.Dtos.Pet;
using Application.Interfaces;
using Application.Services.Utilities;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    public class PetService : Service, IPetService
    {
        public PetService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public async Task<ServiceResponse<PetsGetPetsDtoResponse>> GetPetsAsync()
        {
            var pets = await Context.Pets.ToListAsync();

            var model = new PetsGetPetsDtoResponse
            {
                Pets = Mapper.Map<List<PetForPetsGetPetsDtoResponse>>(pets)
            };
            return new ServiceResponse<PetsGetPetsDtoResponse>(HttpStatusCode.OK, model);
        }

        public async Task<ServiceResponse<PetsGetPetDtoResponse>> GetPetAsync(Guid petId)
        {
            if(petId == Guid.Empty)
                return new ServiceResponse<PetsGetPetDtoResponse>(HttpStatusCode.BadRequest, "Nieprawidłowy Pet Id");

            var username = CurrentlyLoggedUserName;
            if(string.IsNullOrWhiteSpace(username))
                return new ServiceResponse<PetsGetPetDtoResponse>(HttpStatusCode.Unauthorized);

            var user = await Context.Users.SingleOrDefaultAsync(u => u.UserName == username);
            if(user == null)
                return new ServiceResponse<PetsGetPetDtoResponse>(HttpStatusCode.BadRequest, "Nie znaleziono użytkownika");

            var pet = await Context.Pets.SingleOrDefaultAsync(p => p.Id == petId);
            if (pet == null)
                return new ServiceResponse<PetsGetPetDtoResponse>(HttpStatusCode.BadRequest, "Nie znaleziono zwierzaka");

            if (user.Role == Role.Owner)
            {
                var owner = await Context.Owners.SingleOrDefaultAsync(o => o.UserId == user.Id);
                if (owner == null)
                    return new ServiceResponse<PetsGetPetDtoResponse>(HttpStatusCode.BadRequest, "Nie znaleziono opiekuna");

                if (!await Context.OwnerPets.AnyAsync(op => op.PetId == pet.Id && op.OwnerId == owner.Id))
                    return new ServiceResponse<PetsGetPetDtoResponse>(HttpStatusCode.Forbidden);

                var petToReturn = Mapper.Map<PetsGetPetDtoResponse>(pet);
                return new ServiceResponse<PetsGetPetDtoResponse>(HttpStatusCode.OK, petToReturn);
            }

            if (user.Role == Role.Vet)
            {
                var vet = await Context.Vets.SingleOrDefaultAsync(v => v.UserId == user.Id);
                if (vet == null)
                    return new ServiceResponse<PetsGetPetDtoResponse>(HttpStatusCode.BadRequest, "Nie znaleziono weterynarza");

                if (!await Context.VetPets.AnyAsync(vp => vp.PetId == pet.Id && vp.VetId == vet.Id))
                    return new ServiceResponse<PetsGetPetDtoResponse>(HttpStatusCode.Forbidden);

                var petToReturn = Mapper.Map<PetsGetPetDtoResponse>(pet);
                return new ServiceResponse<PetsGetPetDtoResponse>(HttpStatusCode.OK, petToReturn);
            }

            return new ServiceResponse<PetsGetPetDtoResponse>(HttpStatusCode.Forbidden);
        }

        public async Task<ServiceResponse<PetsCreatePetDtoResponse>> CreatePetAsync(PetsCreatePetDtoRequest dto)
        {
            if(dto.Gender == null)
                return new ServiceResponse<PetsCreatePetDtoResponse>(HttpStatusCode.BadRequest, "Należy podać płeć");

            var username = CurrentlyLoggedUserName;
            var user = await Context.Users.SingleOrDefaultAsync(u => u.UserName == username);
            if (user == null)
                return new ServiceResponse<PetsCreatePetDtoResponse>(HttpStatusCode.BadRequest, "Nie znaleziono użytkownika");

            var owner = await Context.Owners.SingleOrDefaultAsync(o => o.UserId == user.Id);
            if (owner == null)
                return new ServiceResponse<PetsCreatePetDtoResponse>(HttpStatusCode.BadRequest, "Nie znaleziono opiekuna");

            if(await Context.Pets.AnyAsync(p => p.Id == dto.Id))
                return new ServiceResponse<PetsCreatePetDtoResponse>(HttpStatusCode.BadRequest, "Istnieje już zwierzak o podanym id.");

            Pet pet = Mapper.Map<Pet>(dto);

            var race = await Context.Races.SingleOrDefaultAsync(r => r.Id == dto.RaceId);
            if (race == null)
                return new ServiceResponse<PetsCreatePetDtoResponse>(HttpStatusCode.BadRequest, "Nieprawidłowa rasa");

            pet.Race = race;

            // if not given from the front
            if (pet.Id == Guid.Empty)
                pet.Id = Guid.NewGuid();

            Context.Pets.Add(pet);
            Context.OwnerPets.Add(new OwnerPet
            {
                Pet = pet,
                Owner = owner
            });

            if (await Context.SaveChangesAsync() <= 0)
                return new ServiceResponse<PetsCreatePetDtoResponse>(HttpStatusCode.BadRequest,
                    "Wystąpił błąd podczas tworzenia zwierzaka");

            var petToReturn = Mapper.Map<PetsCreatePetDtoResponse>(pet);
            return new ServiceResponse<PetsCreatePetDtoResponse>(HttpStatusCode.OK, petToReturn);
        }

        public async Task<ServiceResponse<PetsUpdatePetDtoResponse>> UpdatePetAsync(Guid petId,
            PetsUpdatePetDtoRequest dto)
        {
            if (petId == Guid.Empty)
                return new ServiceResponse<PetsUpdatePetDtoResponse>(HttpStatusCode.BadRequest,
                    "Id zwierzaka nie może być pusty");

            if (dto.Gender == null)
                return new ServiceResponse<PetsUpdatePetDtoResponse>(HttpStatusCode.BadRequest,
                    "Należy podać płeć");

            var username = CurrentlyLoggedUserName;

            var user = await Context.Users.SingleOrDefaultAsync(u => u.UserName == username);
            if (user == null)
                return new ServiceResponse<PetsUpdatePetDtoResponse>(HttpStatusCode.BadRequest, "Nie znaleziono użytkownika");

            var pet = await Context.Pets.SingleOrDefaultAsync(p => p.Id == petId);
            if (pet == null)
                return new ServiceResponse<PetsUpdatePetDtoResponse>(HttpStatusCode.BadRequest, "Nie znaleziono zwierzaka");

            if (user.Role == Role.Owner)
            {
                var owner = await Context.Owners.SingleOrDefaultAsync(o => o.UserId == user.Id);
                if (owner == null)
                    return new ServiceResponse<PetsUpdatePetDtoResponse>(HttpStatusCode.BadRequest, "Nie znaleziono opiekuna");

                if (!await Context.OwnerPets.AnyAsync(op => op.PetId == petId && op.OwnerId == owner.Id))
                    return new ServiceResponse<PetsUpdatePetDtoResponse>(HttpStatusCode.Forbidden);

                Mapper.Map(dto, pet);
                return await Context.SaveChangesAsync() > 0
                    ? new ServiceResponse<PetsUpdatePetDtoResponse>(HttpStatusCode.OK,
                        Mapper.Map<PetsUpdatePetDtoResponse>(dto))
                    : new ServiceResponse<PetsUpdatePetDtoResponse>(HttpStatusCode.BadRequest,
                        "Wystąpił błąd podczas aktualizacji zwierzaka");
            }

            if (user.Role == Role.Vet)
            {
                var vet = await Context.Vets.SingleOrDefaultAsync(v => v.UserId == user.Id);
                if (vet == null)
                    return new ServiceResponse<PetsUpdatePetDtoResponse>(HttpStatusCode.BadRequest, "Nie znaleziono weterynarza");

                if (!await Context.VetPets.AnyAsync(vp => vp.PetId == petId && vp.VetId == vet.Id))
                    return new ServiceResponse<PetsUpdatePetDtoResponse>(HttpStatusCode.Forbidden);

                Mapper.Map(dto, pet);
                return await Context.SaveChangesAsync() > 0
                    ? new ServiceResponse<PetsUpdatePetDtoResponse>(HttpStatusCode.OK,
                        Mapper.Map<PetsUpdatePetDtoResponse>(dto))
                    : new ServiceResponse<PetsUpdatePetDtoResponse>(HttpStatusCode.BadRequest,
                        "Wystąpił błąd podczas aktualizacji zwierzaka");

            }

            if (user.Role == Role.Administrator)
            {
                Mapper.Map(dto, pet);
                return await Context.SaveChangesAsync() > 0
                    ? new ServiceResponse<PetsUpdatePetDtoResponse>(HttpStatusCode.OK,
                        Mapper.Map<PetsUpdatePetDtoResponse>(dto))
                    : new ServiceResponse<PetsUpdatePetDtoResponse>(HttpStatusCode.BadRequest,
                        "Wystąpił błąd podczas aktualizacji zwierzaka");
            }
            return new ServiceResponse<PetsUpdatePetDtoResponse>(HttpStatusCode.Forbidden);
        }

        public async Task<ServiceResponse> DeletePetAsync(Guid petId)
        {
            var username = CurrentlyLoggedUserName;

            var user = await Context.Users.SingleOrDefaultAsync(u => u.UserName == username);
            if (user == null)
                return new ServiceResponse(HttpStatusCode.BadRequest, "Nie znaleziono użytkownika");

            var owner = await Context.Owners.SingleOrDefaultAsync(o => o.UserId == user.Id);
            if (owner == null)
                return new ServiceResponse(HttpStatusCode.BadRequest, "Nie znaleziono opiekuna");

            if (petId == Guid.Empty)
                return new ServiceResponse(HttpStatusCode.BadRequest, "Nieprawidłowy id zwierzaka");

            var pet = await Context.Pets.SingleOrDefaultAsync(p => p.Id== petId);
            if (pet == null)
                return new ServiceResponse(HttpStatusCode.BadRequest, "Nie znaleziono zwierzaka");

            if (!await Context.OwnerPets.AnyAsync(op => op.OwnerId == owner.Id && op.PetId == petId))
                return new ServiceResponse(HttpStatusCode.Forbidden);

            Context.Pets.Remove(pet);
            return await Context.SaveChangesAsync() > 0 ? new ServiceResponse(HttpStatusCode.OK) : new ServiceResponse(HttpStatusCode.BadRequest, "Wystąpił błąd podczas usuwania zwierzaka");
        }

        public async Task<ServiceResponse<PetsGetMyPetsDtoResponse>> GetMyPetsAsync()
        {
            if(CurrentlyLoggedUser == null)
                return new ServiceResponse<PetsGetMyPetsDtoResponse>(HttpStatusCode.Unauthorized);

            var pets = await Context.Pets
                .Where(x => x.OwnerPets
                    .Any(y => y.OwnerId == CurrentlyLoggedUser.Owner.Id))
                .ToListAsync();

            var dto = new PetsGetMyPetsDtoResponse();
            dto.Pets = Mapper.Map<List<PetForPetsGetMyPetsDtoResponse>>(pets);

            return new ServiceResponse<PetsGetMyPetsDtoResponse>(HttpStatusCode.OK, dto);
        }

        public async Task<ServiceResponse<PetsGetSharedPetsDtoResponse>> GetSharedPetsAsync()
        {
            AssignCurrentlyLoggedUser();

            if (CurrentlyLoggedUser == null)
                return new ServiceResponse<PetsGetSharedPetsDtoResponse>(HttpStatusCode.Unauthorized);

            var pets = await Context.Requests
                .Where(x => x.IsAccepted && x.User.Id == CurrentlyLoggedUser.Id)
                .Select(x => x.Pet)
                .ToListAsync();

            var dto = new PetsGetSharedPetsDtoResponse();
            dto.Pets = Mapper.Map<List<PetForPetsGetSharedPetsDtoResponse>>(pets);

            return new ServiceResponse<PetsGetSharedPetsDtoResponse>(HttpStatusCode.OK, dto);
        }
    }
}