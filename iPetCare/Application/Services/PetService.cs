using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Application.Dtos.Pet;
using Application.Interfaces;
using Application.Services.Utilities;
using Domain.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    public class PetService : Service, IPetService
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public PetService(IServiceProvider serviceProvider, IHostingEnvironment hostingEnvironment) : base(
            serviceProvider)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        public async Task<ServiceResponse<GetPetsDtoResponse>> GetPetsAsync()
        {
            var pets = await Context.Pets.ToListAsync();

            var model = new GetPetsDtoResponse
            {
                Pets = Mapper.Map<List<PetForGetPetsDtoResponse>>(pets)
            };
            return new ServiceResponse<GetPetsDtoResponse>(HttpStatusCode.OK, model);
        }

        public async Task<ServiceResponse<GetPetDtoResponse>> GetPetAsync(Guid petId)
        {
            if (petId == Guid.Empty)
                return new ServiceResponse<GetPetDtoResponse>(HttpStatusCode.BadRequest, "Nieprawidłowy Pet Id");

            if (CurrentlyLoggedUser == null)
                return new ServiceResponse<GetPetDtoResponse>(HttpStatusCode.Unauthorized);

            var pet = await Context.Pets.SingleOrDefaultAsync(p => p.Id == petId);
            if (pet == null)
                return new ServiceResponse<GetPetDtoResponse>(HttpStatusCode.NotFound);

            if (CurrentlyLoggedUser.Role == Role.Owner)
            {
                var owner = await Context.Owners.SingleOrDefaultAsync(o => o.UserId == CurrentlyLoggedUser.Id);
                if (owner == null)
                    return new ServiceResponse<GetPetDtoResponse>(HttpStatusCode.Unauthorized);

                if (!await Context.OwnerPets.AnyAsync(op => op.PetId == pet.Id && op.OwnerId == owner.Id))
                    return new ServiceResponse<GetPetDtoResponse>(HttpStatusCode.Forbidden);

                var petToReturn = Mapper.Map<GetPetDtoResponse>(pet);
                return new ServiceResponse<GetPetDtoResponse>(HttpStatusCode.OK, petToReturn);
            }

            if (CurrentlyLoggedUser.Role == Role.Vet)
            {
                var vet = await Context.Vets.SingleOrDefaultAsync(v => v.UserId == CurrentlyLoggedUser.Id);
                if (vet == null)
                    return new ServiceResponse<GetPetDtoResponse>(HttpStatusCode.Unauthorized);

                if (!await Context.VetPets.AnyAsync(vp => vp.PetId == pet.Id && vp.VetId == vet.Id))
                    return new ServiceResponse<GetPetDtoResponse>(HttpStatusCode.Forbidden);

                var petToReturn = Mapper.Map<GetPetDtoResponse>(pet);
                return new ServiceResponse<GetPetDtoResponse>(HttpStatusCode.OK, petToReturn);
            }

            if (CurrentlyLoggedUser.Role == Role.Administrator)
            {
                var petToReturn = Mapper.Map<GetPetDtoResponse>(pet);
                return new ServiceResponse<GetPetDtoResponse>(HttpStatusCode.OK, petToReturn);
            }

            return new ServiceResponse<GetPetDtoResponse>(HttpStatusCode.Forbidden);
        }

        public async Task<ServiceResponse<CreatePetDtoResponse>> CreatePetAsync(CreatePetDtoRequest dto)
        {
            if (dto.Gender == null)
                return new ServiceResponse<CreatePetDtoResponse>(HttpStatusCode.BadRequest, "Należy podać płeć");

            if (CurrentlyLoggedUser == null)
                return new ServiceResponse<CreatePetDtoResponse>(HttpStatusCode.Unauthorized);

            var owner = await Context.Owners.SingleOrDefaultAsync(o => o.UserId == CurrentlyLoggedUser.Id);
            if (owner == null)
                return new ServiceResponse<CreatePetDtoResponse>(HttpStatusCode.Unauthorized);

            // if not given from the front
            if (dto.Id == Guid.Empty)
                dto.Id = Guid.NewGuid();

            if (await Context.Pets.AnyAsync(p => p.Id == dto.Id))
                return new ServiceResponse<CreatePetDtoResponse>(HttpStatusCode.BadRequest,
                    "Istnieje już zwierzak o podanym id.");

            var pet = Mapper.Map<Pet>(dto);

            var race = await Context.Races.SingleOrDefaultAsync(r => r.Id == dto.RaceId);
            if (race == null)
                return new ServiceResponse<CreatePetDtoResponse>(HttpStatusCode.BadRequest, "Nieprawidłowa rasa");

            pet.Race = race;
            var imageAssigned = await ChangePetImageAsync(dto.Image, pet);

            Context.Pets.Add(pet);
            Context.OwnerPets.Add(new OwnerPet
            {
                Pet = pet,
                Owner = owner
            });


            var result = await Context.SaveChangesAsync();

            if (result > 0 || imageAssigned)
                return new ServiceResponse<CreatePetDtoResponse>(HttpStatusCode.OK,
                    Mapper.Map<CreatePetDtoResponse>(pet));

            return new ServiceResponse<CreatePetDtoResponse>(HttpStatusCode.BadRequest,
                "Wystąpił błąd podczas tworzenia zwierzaka");
        }

        public async Task<ServiceResponse<UpdatePetDtoResponse>> UpdatePetAsync(Guid petId,
            UpdatePetDtoRequest dto)
        {
            if (petId == Guid.Empty)
                return new ServiceResponse<UpdatePetDtoResponse>(HttpStatusCode.BadRequest,
                    "Id zwierzaka nie może być pusty");

            if (dto.Gender == null)
                return new ServiceResponse<UpdatePetDtoResponse>(HttpStatusCode.BadRequest,
                    "Należy podać płeć");

            if (CurrentlyLoggedUser == null)
                return new ServiceResponse<UpdatePetDtoResponse>(HttpStatusCode.Unauthorized);

            var pet = await Context.Pets.SingleOrDefaultAsync(p => p.Id == petId);
            if (pet == null)
                return new ServiceResponse<UpdatePetDtoResponse>(HttpStatusCode.NotFound);

            if (CurrentlyLoggedUser.Role == Role.Owner)
            {
                var owner = await Context.Owners.SingleOrDefaultAsync(o => o.UserId == CurrentlyLoggedUser.Id);
                if (owner == null)
                    return new ServiceResponse<UpdatePetDtoResponse>(HttpStatusCode.Unauthorized);

                if (!await Context.OwnerPets.AnyAsync(op => op.PetId == petId && op.OwnerId == owner.Id))
                    return new ServiceResponse<UpdatePetDtoResponse>(HttpStatusCode.Forbidden);

                return await ChangePetInfoWithImageAsync(dto, pet);
            }

            if (CurrentlyLoggedUser.Role == Role.Vet)
            {
                var vet = await Context.Vets.SingleOrDefaultAsync(v => v.UserId == CurrentlyLoggedUser.Id);
                if (vet == null)
                    return new ServiceResponse<UpdatePetDtoResponse>(HttpStatusCode.Unauthorized);

                if (!await Context.VetPets.AnyAsync(vp => vp.PetId == petId && vp.VetId == vet.Id))
                    return new ServiceResponse<UpdatePetDtoResponse>(HttpStatusCode.Forbidden);

                return await ChangePetInfoAsync(dto, pet);
            }

            if (CurrentlyLoggedUser.Role == Role.Administrator) return await ChangePetInfoWithImageAsync(dto, pet);
            return new ServiceResponse<UpdatePetDtoResponse>(HttpStatusCode.Forbidden);
        }

        public async Task<ServiceResponse> DeletePetAsync(Guid petId)
        {
            if (petId == Guid.Empty)
                return new ServiceResponse(HttpStatusCode.BadRequest, "Nieprawidłowy id zwierzaka");

            if (CurrentlyLoggedUser == null)
                return new ServiceResponse(HttpStatusCode.Unauthorized);

            var pet = await Context.Pets.SingleOrDefaultAsync(p => p.Id == petId);
            if (pet == null)
                return new ServiceResponse(HttpStatusCode.NotFound);

            if (CurrentlyLoggedUser.Role == Role.Administrator)
            {
                Context.Pets.Remove(pet);
            }
            else
            {
                var owner = await Context.Owners.SingleOrDefaultAsync(o => o.UserId == CurrentlyLoggedUser.Id);
                if (owner == null)
                    return new ServiceResponse(HttpStatusCode.Unauthorized);

                if (!await Context.OwnerPets.AnyAsync(op => op.OwnerId == owner.Id && op.PetId == petId))
                    return new ServiceResponse(HttpStatusCode.Forbidden);

                Context.Pets.Remove(pet);
            }

            return await Context.SaveChangesAsync() > 0
                ? new ServiceResponse(HttpStatusCode.OK)
                : new ServiceResponse(HttpStatusCode.BadRequest, "Wystąpił błąd podczas usuwania zwierzaka");
        }

        public async Task<ServiceResponse<GetMyPetsDtoResponse>> GetMyPetsAsync()
        {
            if (CurrentlyLoggedUser == null)
                return new ServiceResponse<GetMyPetsDtoResponse>(HttpStatusCode.Unauthorized);

            var pets = await Context.Pets
                .Where(x => x.OwnerPets
                    .Any(y => y.OwnerId == CurrentlyLoggedUser.Owner.Id))
                .ToListAsync();

            var dto = new GetMyPetsDtoResponse();
            dto.Pets = Mapper.Map<List<PetForGetMyPetsDtoResponse>>(pets);

            return new ServiceResponse<GetMyPetsDtoResponse>(HttpStatusCode.OK, dto);
        }

        public async Task<ServiceResponse<GetSharedPetsDtoResponse>> GetSharedPetsAsync()
        {
            if (CurrentlyLoggedUser == null)
                return new ServiceResponse<GetSharedPetsDtoResponse>(HttpStatusCode.Unauthorized);

            if (CurrentlyLoggedUser == null)
                return new ServiceResponse<GetSharedPetsDtoResponse>(HttpStatusCode.Unauthorized);

            var pets = await Context.Requests
                .Where(x => x.IsAccepted && x.User.Id == CurrentlyLoggedUser.Id)
                .Select(x => x.Pet)
                .ToListAsync();

            var dto = new GetSharedPetsDtoResponse();
            dto.Pets = Mapper.Map<List<PetForGetSharedPetsDtoResponse>>(pets);

            return new ServiceResponse<GetSharedPetsDtoResponse>(HttpStatusCode.OK, dto);
        }

        private async Task<ServiceResponse<UpdatePetDtoResponse>> ChangePetInfoAsync(UpdatePetDtoRequest dto, Pet pet)
        {
            Mapper.Map(dto, pet);
            return await Context.SaveChangesAsync() > 0
                ? new ServiceResponse<UpdatePetDtoResponse>(HttpStatusCode.OK,
                    Mapper.Map<UpdatePetDtoResponse>(dto))
                : new ServiceResponse<UpdatePetDtoResponse>(HttpStatusCode.BadRequest,
                    "Wystąpił błąd podczas aktualizacji zwierzaka");
        }

        private async Task<ServiceResponse<UpdatePetDtoResponse>> ChangePetInfoWithImageAsync(UpdatePetDtoRequest dto,
            Pet pet)
        {
            Mapper.Map(dto, pet);

            var imageChanged = await ChangePetImageAsync(dto.Image, pet);

            var result = await Context.SaveChangesAsync();
            if (result > 0 || imageChanged)
                return new ServiceResponse<UpdatePetDtoResponse>(HttpStatusCode.OK,
                    Mapper.Map<UpdatePetDtoResponse>(dto));

            return new ServiceResponse<UpdatePetDtoResponse>(HttpStatusCode.BadRequest,
                "Wystąpił błąd podczas aktualizacji zwierzaka");
        }

        private async Task<bool> ChangePetImageAsync(IFormFile image, Pet pet)
        {
            if (image == null || image.Length <= 0) return false;

            var fileExtension = Path.GetExtension(image.FileName);

            var imageFolderPath = "Uploads/Pets/Photos";

            // create folder it should upload files to
            Directory.CreateDirectory($"{_hostingEnvironment.WebRootPath}/{imageFolderPath}");

            // find images named as pet id, regardless of the extension
            var files = Directory.GetFiles($"{_hostingEnvironment.WebRootPath}/{imageFolderPath}", $"{pet.Id}.*");

            // if found any files
            if (files.Length > 0)
                // delete them
                foreach (var file in files)
                    File.Delete(file);

            var newFileName = $"{pet.Id}{fileExtension}";

            var filePath = Path.Combine(_hostingEnvironment.WebRootPath, imageFolderPath, newFileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }

            pet.ImageUrl = $"/{imageFolderPath}/{newFileName}";

            return true;
        }
    }
}