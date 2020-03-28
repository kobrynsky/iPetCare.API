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


        public async Task<ServiceResponse<SpeciesCreateSpeciesDtoResponse>> CreateAsync(SpeciesCreateSpeciesDtoRequest dto)
        {
            if (await Context.Species.Where(x => x.Name == dto.Name).AnyAsync())
                return new ServiceResponse<SpeciesCreateSpeciesDtoResponse>(HttpStatusCode.BadRequest, "Podany gatunek już istnieje");

            if(CurrentlyLoggedUser == null)
                return new ServiceResponse<SpeciesCreateSpeciesDtoResponse>(HttpStatusCode.Unauthorized);

            if(CurrentlyLoggedUser.Role != Role.Administrator)
                return new ServiceResponse<SpeciesCreateSpeciesDtoResponse>(HttpStatusCode.Forbidden);

            var species = new Species
            {
                Name = dto.Name
            };

            Context.Species.Add(species);

            var responseDto = new SpeciesCreateSpeciesDtoResponse()
            {
                Name = species.Name,
                Id = species.Id
            };

            return await Context.SaveChangesAsync() > 0
                ? new ServiceResponse<SpeciesCreateSpeciesDtoResponse>(HttpStatusCode.OK, responseDto)
                : new ServiceResponse<SpeciesCreateSpeciesDtoResponse>(HttpStatusCode.BadRequest, "Wystąpił błąd podczas zapisu");
        }

        public async Task<ServiceResponse<SpeciesDeleteSpeciesDtoResponse>> DeleteAsync(int speciesId)
        {
            if (CurrentlyLoggedUser == null)
                return new ServiceResponse<SpeciesDeleteSpeciesDtoResponse>(HttpStatusCode.Unauthorized);

            if (CurrentlyLoggedUser.Role != Role.Administrator)
                return new ServiceResponse<SpeciesDeleteSpeciesDtoResponse>(HttpStatusCode.Forbidden);

            var species = Context.Species.Find(speciesId);

            if (species == null)
                return new ServiceResponse<SpeciesDeleteSpeciesDtoResponse>(HttpStatusCode.NotFound);

            Context.Species.Remove(species);

            var dto = Mapper.Map<SpeciesDeleteSpeciesDtoResponse>(species);

            return await Context.SaveChangesAsync() > 0
                ? new ServiceResponse<SpeciesDeleteSpeciesDtoResponse>(HttpStatusCode.OK, dto)
                : new ServiceResponse<SpeciesDeleteSpeciesDtoResponse>(HttpStatusCode.BadRequest, "Wystąpił błąd podczas zapisu");
        }

        public async Task<ServiceResponse<SpeciesGetAllSpeciesDtoResponse>> GetAllAsync()
        {
            if (CurrentlyLoggedUser == null)
                return new ServiceResponse<SpeciesGetAllSpeciesDtoResponse>(HttpStatusCode.Unauthorized);

            var species = await Context.Species.ToListAsync();

            var dto = new SpeciesGetAllSpeciesDtoResponse
            {
                Species = Mapper.Map<List<SpeciesDetailGetAllDtoResponse>>(species)
            };

            return new ServiceResponse<SpeciesGetAllSpeciesDtoResponse>(HttpStatusCode.OK, dto);
        }

        public async Task<ServiceResponse<SpeciesGetSpeciesDtoResponse>> GetAsync(int speciesId)
        {
            if (CurrentlyLoggedUser == null)
                return new ServiceResponse<SpeciesGetSpeciesDtoResponse>(HttpStatusCode.Unauthorized);

            var species = await Context.Species.FindAsync(speciesId);

            if (species == null)
                return new ServiceResponse<SpeciesGetSpeciesDtoResponse>(HttpStatusCode.NotFound);

            var dto = Mapper.Map<SpeciesGetSpeciesDtoResponse>(species);

            var races = await Context.Races.ToListAsync();

            var filteredRaces = races.Where(race => race.SpeciesId == speciesId).ToList();

            dto.Races = Mapper.Map<List<RaceDetailsGetDtoResponse>>(filteredRaces);

            return new ServiceResponse<SpeciesGetSpeciesDtoResponse>(HttpStatusCode.OK, dto);
        }

        public async Task<ServiceResponse<SpeciesUpdateSpeciesDtoResponse>> UpdateAsync(int speciesId, SpeciesUpdateSpeciesDtoRequest dto)
        {
            if (CurrentlyLoggedUser == null)
                return new ServiceResponse<SpeciesUpdateSpeciesDtoResponse>(HttpStatusCode.Unauthorized);

            if (CurrentlyLoggedUser.Role != Role.Administrator)
                return new ServiceResponse<SpeciesUpdateSpeciesDtoResponse>(HttpStatusCode.Forbidden);

            if (await Context.Species.Where(x => x.Name == dto.Name).AnyAsync())
                return new ServiceResponse<SpeciesUpdateSpeciesDtoResponse>(HttpStatusCode.BadRequest, "Podany gatunek już istnieje");

            var species = Context.Species.Find(speciesId);

            if (species == null)
                return new ServiceResponse<SpeciesUpdateSpeciesDtoResponse>(HttpStatusCode.NotFound);

            var responseDto = Mapper.Map<SpeciesUpdateSpeciesDtoResponse>(species);

            if (species.Name.Equals(dto.Name))
                return new ServiceResponse<SpeciesUpdateSpeciesDtoResponse>(HttpStatusCode.OK, responseDto);

            species.Name = dto.Name;

            return await Context.SaveChangesAsync() > 0
                ? new ServiceResponse<SpeciesUpdateSpeciesDtoResponse>(HttpStatusCode.OK, responseDto)
                : new ServiceResponse<SpeciesUpdateSpeciesDtoResponse>(HttpStatusCode.BadRequest, "Wystąpił błąd podczas zapisu");
        }
    }
}
