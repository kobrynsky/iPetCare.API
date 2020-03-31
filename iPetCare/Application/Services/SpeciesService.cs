using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Application.Dtos.Species;
using Application.Interfaces;
using Application.Services.Utilities;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    public class SpeciesService : Service, ISpeciesService
    {

        public SpeciesService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public async Task<ServiceResponse<CreateSpeciesDtoResponse>> CreateAsync(CreateSpeciesDtoRequest dto)
        {
            if (await Context.Species.Where(x => x.Name == dto.Name).AnyAsync())
                return new ServiceResponse<CreateSpeciesDtoResponse>(HttpStatusCode.BadRequest, "Podany gatunek już istnieje");

            if(CurrentlyLoggedUser == null)
                return new ServiceResponse<CreateSpeciesDtoResponse>(HttpStatusCode.Unauthorized);

            if(CurrentlyLoggedUser.Role != Role.Administrator)
                return new ServiceResponse<CreateSpeciesDtoResponse>(HttpStatusCode.Forbidden);

            var species = new Species
            {
                Name = dto.Name
            };

            Context.Species.Add(species);

            var responseDto = new CreateSpeciesDtoResponse()
            {
                Name = species.Name,
                Id = species.Id
            };

            return await Context.SaveChangesAsync() > 0
                ? new ServiceResponse<CreateSpeciesDtoResponse>(HttpStatusCode.OK, responseDto)
                : new ServiceResponse<CreateSpeciesDtoResponse>(HttpStatusCode.BadRequest, "Wystąpił błąd podczas zapisu");
        }

        public async Task<ServiceResponse<DeleteSpeciesDtoResponse>> DeleteAsync(int speciesId)
        {
            if (CurrentlyLoggedUser == null)
                return new ServiceResponse<DeleteSpeciesDtoResponse>(HttpStatusCode.Unauthorized);

            if (CurrentlyLoggedUser.Role != Role.Administrator)
                return new ServiceResponse<DeleteSpeciesDtoResponse>(HttpStatusCode.Forbidden);

            var species = Context.Species.Find(speciesId);

            if (species == null)
                return new ServiceResponse<DeleteSpeciesDtoResponse>(HttpStatusCode.NotFound);

            Context.Species.Remove(species);

            var dto = Mapper.Map<DeleteSpeciesDtoResponse>(species);

            return await Context.SaveChangesAsync() > 0
                ? new ServiceResponse<DeleteSpeciesDtoResponse>(HttpStatusCode.OK, dto)
                : new ServiceResponse<DeleteSpeciesDtoResponse>(HttpStatusCode.BadRequest, "Wystąpił błąd podczas zapisu");
        }

        public async Task<ServiceResponse<GetAllSpeciesDtoResponse>> GetAllAsync()
        {
            if (CurrentlyLoggedUser == null)
                return new ServiceResponse<GetAllSpeciesDtoResponse>(HttpStatusCode.Unauthorized);

            var species = await Context.Species.ToListAsync();

            var dto = new GetAllSpeciesDtoResponse
            {
                Species = Mapper.Map<List<SpeciesForGetAllSpeciesDtoResponse>>(species)
            };

            return new ServiceResponse<GetAllSpeciesDtoResponse>(HttpStatusCode.OK, dto);
        }

        public async Task<ServiceResponse<GetSpeciesDtoResponse>> GetAsync(int speciesId)
        {
            if (CurrentlyLoggedUser == null)
                return new ServiceResponse<GetSpeciesDtoResponse>(HttpStatusCode.Unauthorized);

            var species = await Context.Species.FindAsync(speciesId);

            if (species == null)
                return new ServiceResponse<GetSpeciesDtoResponse>(HttpStatusCode.NotFound);

            var dto = Mapper.Map<GetSpeciesDtoResponse>(species);

            var races = await Context.Races.ToListAsync();

            var filteredRaces = races.Where(race => race.SpeciesId == speciesId).ToList();

            dto.Races = Mapper.Map<List<RaceForGetSpeciesDtoResponse>>(filteredRaces);

            return new ServiceResponse<GetSpeciesDtoResponse>(HttpStatusCode.OK, dto);
        }

        public async Task<ServiceResponse<UpdateSpeciesDtoResponse>> UpdateAsync(int speciesId, UpdateSpeciesDtoRequest dto)
        {
            if (CurrentlyLoggedUser == null)
                return new ServiceResponse<UpdateSpeciesDtoResponse>(HttpStatusCode.Unauthorized);

            if (CurrentlyLoggedUser.Role != Role.Administrator)
                return new ServiceResponse<UpdateSpeciesDtoResponse>(HttpStatusCode.Forbidden);

            if (await Context.Species.Where(x => x.Name == dto.Name).AnyAsync())
                return new ServiceResponse<UpdateSpeciesDtoResponse>(HttpStatusCode.BadRequest, "Podany gatunek już istnieje");

            var species = Context.Species.Find(speciesId);

            if (species == null)
                return new ServiceResponse<UpdateSpeciesDtoResponse>(HttpStatusCode.NotFound);

            var responseDto = Mapper.Map<UpdateSpeciesDtoResponse>(species);

            species.Name = dto.Name;

            return await Context.SaveChangesAsync() > 0
                ? new ServiceResponse<UpdateSpeciesDtoResponse>(HttpStatusCode.OK, responseDto)
                : new ServiceResponse<UpdateSpeciesDtoResponse>(HttpStatusCode.BadRequest, "Wystąpił błąd podczas zapisu");
        }
    }
}
