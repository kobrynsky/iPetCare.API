using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Application.Dtos.Races;
using Application.Interfaces;
using Application.Services.Utilities;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    public class RaceService : Service, IRaceService
    {

        public RaceService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
        public async Task<ServiceResponse<RaceCreateDtoResponse>> CreateAsync(RaceCreateDtoRequest dto)
        {
            if (await Context.Races.Where(x => x.Name == dto.Name).AnyAsync())
                return new ServiceResponse<RaceCreateDtoResponse>(HttpStatusCode.BadRequest, "Podana rasa już istnieje");

            if(CurrentlyLoggedUser == null)
                return new ServiceResponse<RaceCreateDtoResponse>(HttpStatusCode.Unauthorized);

            if(CurrentlyLoggedUser.Role != Role.Administrator)
                return new ServiceResponse<RaceCreateDtoResponse>(HttpStatusCode.Forbidden);

            var race = new Race()
            {
                Name = dto.Name,
                SpeciesId = dto.SpeciesId
            };

            var responseDto = new RaceCreateDtoResponse()
            {
                Name = race.Name,
                SpeciesId = race.SpeciesId,
                Id = race.Id
            };

            Context.Races.Add(race);
            var result = await Context.SaveChangesAsync();

            return result > 0
                ? new ServiceResponse<RaceCreateDtoResponse>(HttpStatusCode.OK, responseDto)
                : new ServiceResponse<RaceCreateDtoResponse>(HttpStatusCode.BadRequest, "Wystąpił błąd podczas tworzenia rasy");
        }

        public async Task<ServiceResponse<RaceGetAllDtoResponse>> GetAllAsync()
        {
            if (CurrentlyLoggedUser == null)
                return new ServiceResponse<RaceGetAllDtoResponse>(HttpStatusCode.Unauthorized);

            var races = await Context.Races.ToListAsync();

            var dto = new RaceGetAllDtoResponse {Races = Mapper.Map<List<RaceDetailGetAllDtoResponse>>(races)};

            return new ServiceResponse<RaceGetAllDtoResponse>(HttpStatusCode.OK, dto);
        }

        public async Task<ServiceResponse<RaceGetDtoResponse>> GetAsync(int raceId)
        {
            if (CurrentlyLoggedUser == null)
                return new ServiceResponse<RaceGetDtoResponse>(HttpStatusCode.Unauthorized);

            var race = await Context.Races.FindAsync(raceId);

            if (race == null)
                return new ServiceResponse<RaceGetDtoResponse>(HttpStatusCode.BadRequest, "Nie istnieje taka rasa w bazie danych");

            var dto = Mapper.Map<RaceGetDtoResponse>(race);

            return new ServiceResponse<RaceGetDtoResponse>(HttpStatusCode.OK, dto);
        }

        public async Task<ServiceResponse<RaceUpdateDtoResponse>> UpdateAsync(int raceId, RaceUpdateDtoRequest dto)
        {
            if(CurrentlyLoggedUser == null)
                return new ServiceResponse<RaceUpdateDtoResponse>(HttpStatusCode.Unauthorized);

            if(CurrentlyLoggedUser.Role != Role.Administrator)
                return new ServiceResponse<RaceUpdateDtoResponse>(HttpStatusCode.Forbidden);

            if (await Context.Races.Where(x => x.Name == dto.Name).AnyAsync())
                return new ServiceResponse<RaceUpdateDtoResponse>(HttpStatusCode.BadRequest, "Podana rasa już istnieje");

            var race = Context.Races.Find(raceId);
            if (race == null)
                return new ServiceResponse<RaceUpdateDtoResponse>(HttpStatusCode.NotFound);

            var species = Context.Species.Find(dto.SpeciesId);
            if (species == null)
                return new ServiceResponse<RaceUpdateDtoResponse>(HttpStatusCode.BadRequest, "Nie istnieje taki gatunek w bazie danych");

            race.Name = dto.Name;
            race.SpeciesId = dto.SpeciesId;

            var responseDto = Mapper.Map<RaceUpdateDtoResponse>(race);

            return await Context.SaveChangesAsync() > 0
                ? new ServiceResponse<RaceUpdateDtoResponse>(HttpStatusCode.OK, responseDto)
                : new ServiceResponse<RaceUpdateDtoResponse>(HttpStatusCode.BadRequest, "Wystąpił błąd podczas zapisu");
        }

        public async Task<ServiceResponse<RaceDeleteDtoResponse>> DeleteAsync(int raceId)
        {
            if(CurrentlyLoggedUser == null)
                return new ServiceResponse<RaceDeleteDtoResponse>(HttpStatusCode.Unauthorized);

            if(CurrentlyLoggedUser.Role != Role.Administrator)
                return new ServiceResponse<RaceDeleteDtoResponse>(HttpStatusCode.Forbidden);

            var race = Context.Races.Find(raceId);

            if (race == null)
                return new ServiceResponse<RaceDeleteDtoResponse>(HttpStatusCode.NotFound);

            var dto = Mapper.Map<RaceDeleteDtoResponse>(race);
            Context.Races.Remove(race);

            return await Context.SaveChangesAsync() > 0
                ? new ServiceResponse<RaceDeleteDtoResponse>(HttpStatusCode.OK, dto)
                : new ServiceResponse<RaceDeleteDtoResponse>(HttpStatusCode.BadRequest, "Wystąpił błąd podczas zapisu");
        }
    }
}
