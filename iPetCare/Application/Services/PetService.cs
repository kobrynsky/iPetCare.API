using System;
using System.Collections.Generic;
using System.Net;
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
                return new ServiceResponse<PetsGetPetDtoResponse>(HttpStatusCode.BadRequest, "Invalid pet id");

            var username = CurrentlyLoggedUserName;
            if(string.IsNullOrWhiteSpace(username))
                return new ServiceResponse<PetsGetPetDtoResponse>(HttpStatusCode.Unauthorized);

            var user = await Context.Users.SingleOrDefaultAsync(u => u.UserName == username);
            if(user == null)
                return new ServiceResponse<PetsGetPetDtoResponse>(HttpStatusCode.BadRequest, "User not found");

            var pet = await Context.Pets.SingleOrDefaultAsync(p => p.Id == petId);
            if (pet == null)
                return new ServiceResponse<PetsGetPetDtoResponse>(HttpStatusCode.BadRequest, "Pet not found");

            if (user.Role == Role.Owner)
            {
                var owner = await Context.Owners.SingleOrDefaultAsync(o => o.UserId == user.Id);
                if (owner == null)
                    return new ServiceResponse<PetsGetPetDtoResponse>(HttpStatusCode.BadRequest, "Owner not found");

                if (!await Context.OwnerPets.AnyAsync(op => op.PetId == pet.Id && op.OwnerId == owner.Id))
                    return new ServiceResponse<PetsGetPetDtoResponse>(HttpStatusCode.Forbidden);

                var petToReturn = Mapper.Map<PetsGetPetDtoResponse>(pet);
                return new ServiceResponse<PetsGetPetDtoResponse>(HttpStatusCode.OK, petToReturn);
            }

            if (user.Role == Role.Vet)
            {
                var vet = await Context.Vets.SingleOrDefaultAsync(v => v.UserId == user.Id);
                if (vet == null)
                    return new ServiceResponse<PetsGetPetDtoResponse>(HttpStatusCode.BadRequest, "Vet not found");

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
                return new ServiceResponse<PetsCreatePetDtoResponse>(HttpStatusCode.BadRequest, "Gender must be specified");

            var username = CurrentlyLoggedUserName;
            var user = await Context.Users.SingleOrDefaultAsync(u => u.UserName == username);
            if (user == null)
                return new ServiceResponse<PetsCreatePetDtoResponse>(HttpStatusCode.BadRequest, "User not found");

            var owner = await Context.Owners.SingleOrDefaultAsync(o => o.UserId == user.Id);
            if (owner == null)
                return new ServiceResponse<PetsCreatePetDtoResponse>(HttpStatusCode.BadRequest, "Owner not found");

            if(await Context.Pets.AnyAsync(p => p.Id == dto.Id))
                return new ServiceResponse<PetsCreatePetDtoResponse>(HttpStatusCode.BadRequest, "Pet with given id already exists, try again with other id");

            Pet pet = Mapper.Map<Pet>(dto);

            var race = await Context.Races.SingleOrDefaultAsync(r => r.Id == dto.RaceId);
            if (race == null)
                return new ServiceResponse<PetsCreatePetDtoResponse>(HttpStatusCode.BadRequest, "Invalid race");

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
                    "An error occured while creating pet");

            var petToReturn = Mapper.Map<PetsCreatePetDtoResponse>(pet);
            return new ServiceResponse<PetsCreatePetDtoResponse>(HttpStatusCode.OK, petToReturn);
        }

        public async Task<ServiceResponse<PetsUpdatePetDtoResponse>> UpdatePetAsync(Guid petId,
            PetsUpdatePetDtoRequest dto)
        {
            if (petId == Guid.Empty)
                return new ServiceResponse<PetsUpdatePetDtoResponse>(HttpStatusCode.BadRequest,
                    "Pet id must be specified");

            if (petId != dto.Id)
                return new ServiceResponse<PetsUpdatePetDtoResponse>(HttpStatusCode.BadRequest, "Pet ids mismatch");

            if (dto.Gender == null)
                return new ServiceResponse<PetsUpdatePetDtoResponse>(HttpStatusCode.BadRequest,
                    "Gender must be specified");

            var username = CurrentlyLoggedUserName;

            var user = await Context.Users.SingleOrDefaultAsync(u => u.UserName == username);
            if (user == null)
                return new ServiceResponse<PetsUpdatePetDtoResponse>(HttpStatusCode.BadRequest, "User not found");

            var pet = await Context.Pets.SingleOrDefaultAsync(p => p.Id == dto.Id);
            if (pet == null)
                return new ServiceResponse<PetsUpdatePetDtoResponse>(HttpStatusCode.BadRequest, "Pet not found");

            if (user.Role == Role.Owner)
            {
                var owner = await Context.Owners.SingleOrDefaultAsync(o => o.UserId == user.Id);
                if (owner == null)
                    return new ServiceResponse<PetsUpdatePetDtoResponse>(HttpStatusCode.BadRequest, "Owner not found");

                if (!await Context.OwnerPets.AnyAsync(op => op.PetId == dto.Id && op.OwnerId == owner.Id))
                    return new ServiceResponse<PetsUpdatePetDtoResponse>(HttpStatusCode.Forbidden);

                Mapper.Map(dto, pet);
                return await Context.SaveChangesAsync() > 0
                    ? new ServiceResponse<PetsUpdatePetDtoResponse>(HttpStatusCode.OK,
                        Mapper.Map<PetsUpdatePetDtoResponse>(dto))
                    : new ServiceResponse<PetsUpdatePetDtoResponse>(HttpStatusCode.BadRequest,
                        "An error occured while updating pet");

            }

            if (user.Role == Role.Vet)
            {
                var vet = await Context.Vets.SingleOrDefaultAsync(v => v.UserId == user.Id);
                if (vet == null)
                    return new ServiceResponse<PetsUpdatePetDtoResponse>(HttpStatusCode.BadRequest, "Vet not found");

                if (!await Context.VetPets.AnyAsync(vp => vp.PetId == dto.Id && vp.VetId == vet.Id))
                    return new ServiceResponse<PetsUpdatePetDtoResponse>(HttpStatusCode.Forbidden);

                Mapper.Map(dto, pet);
                return await Context.SaveChangesAsync() > 0
                    ? new ServiceResponse<PetsUpdatePetDtoResponse>(HttpStatusCode.OK,
                        Mapper.Map<PetsUpdatePetDtoResponse>(dto))
                    : new ServiceResponse<PetsUpdatePetDtoResponse>(HttpStatusCode.BadRequest,
                        "An error occured while updating pet");

            }

            if (user.Role == Role.Administrator)
            {
                Mapper.Map(dto, pet);
                return await Context.SaveChangesAsync() > 0
                    ? new ServiceResponse<PetsUpdatePetDtoResponse>(HttpStatusCode.OK,
                        Mapper.Map<PetsUpdatePetDtoResponse>(dto))
                    : new ServiceResponse<PetsUpdatePetDtoResponse>(HttpStatusCode.BadRequest,
                        "An error occured while updating pet");
            }
            return new ServiceResponse<PetsUpdatePetDtoResponse>(HttpStatusCode.Forbidden);
        }

        public async Task<ServiceResponse> DeletePetAsync(Guid petId)
        {
            var username = CurrentlyLoggedUserName;

            var user = await Context.Users.SingleOrDefaultAsync(u => u.UserName == username);
            if (user == null)
                return new ServiceResponse(HttpStatusCode.BadRequest, "User not found");

            var owner = await Context.Owners.SingleOrDefaultAsync(o => o.UserId == user.Id);
            if (owner == null)
                return new ServiceResponse(HttpStatusCode.BadRequest, "Owner not found");

            if (petId == Guid.Empty)
                return new ServiceResponse(HttpStatusCode.BadRequest, "Invalid pet id");

            var pet = await Context.Pets.SingleOrDefaultAsync(p => p.Id== petId);
            if (pet == null)
                return new ServiceResponse(HttpStatusCode.BadRequest, "Pet not found");

            if (!await Context.OwnerPets.AnyAsync(op => op.OwnerId == owner.Id && op.PetId == petId))
                return new ServiceResponse(HttpStatusCode.Forbidden);

            Context.Pets.Remove(pet);
            return await Context.SaveChangesAsync() > 0 ? new ServiceResponse(HttpStatusCode.OK) : new ServiceResponse(HttpStatusCode.BadRequest, "An error occured while deleteing pet");
        }
    }
}