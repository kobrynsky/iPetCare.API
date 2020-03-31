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
        public async Task<ServiceResponse<CreateRaceDtoResponse>> CreateAsync(CreateRaceDtoRequest dto)
        {
            if (await Context.Races.Where(x => x.Name == dto.Name).AnyAsync())
                return new ServiceResponse<CreateRaceDtoResponse>(HttpStatusCode.BadRequest, "Podana rasa już istnieje");

            if(CurrentlyLoggedUser == null)
                return new ServiceResponse<CreateRaceDtoResponse>(HttpStatusCode.Unauthorized);

            if(CurrentlyLoggedUser.Role != Role.Administrator)
                return new ServiceResponse<CreateRaceDtoResponse>(HttpStatusCode.Forbidden);

            var race = new Race()
            {
                Name = dto.Name,
                SpeciesId = dto.SpeciesId
            };

            var responseDto = new CreateRaceDtoResponse()
            {
                Name = race.Name,
                SpeciesId = race.SpeciesId,
                Id = race.Id
            };

            Context.Races.Add(race);
            var result = await Context.SaveChangesAsync();

            return result > 0
                ? new ServiceResponse<CreateRaceDtoResponse>(HttpStatusCode.OK, responseDto)
                : new ServiceResponse<CreateRaceDtoResponse>(HttpStatusCode.BadRequest, "Wystąpił błąd podczas tworzenia rasy");
        }

        public async Task<ServiceResponse<GetAllRacesDtoResponse>> GetAllAsync()
        {
            if (CurrentlyLoggedUser == null)
                return new ServiceResponse<GetAllRacesDtoResponse>(HttpStatusCode.Unauthorized);

            var races = await Context.Races.ToListAsync();

            var dto = new GetAllRacesDtoResponse {Races = Mapper.Map<List<RaceForGetAllRacesDtoResponse>>(races)};

            return new ServiceResponse<GetAllRacesDtoResponse>(HttpStatusCode.OK, dto);
        }

        public async Task<ServiceResponse<GetRaceDtoResponse>> GetAsync(int raceId)
        {
            if (CurrentlyLoggedUser == null)
                return new ServiceResponse<GetRaceDtoResponse>(HttpStatusCode.Unauthorized);

            var race = await Context.Races.FindAsync(raceId);

            if (race == null)
                return new ServiceResponse<GetRaceDtoResponse>(HttpStatusCode.BadRequest, "Nie istnieje taka rasa w bazie danych");

            var dto = Mapper.Map<GetRaceDtoResponse>(race);

            return new ServiceResponse<GetRaceDtoResponse>(HttpStatusCode.OK, dto);
        }

        public async Task<ServiceResponse<UpdateRaceDtoResponse>> UpdateAsync(int raceId, UpdateRaceDtoRequest dto)
        {
            if(CurrentlyLoggedUser == null)
                return new ServiceResponse<UpdateRaceDtoResponse>(HttpStatusCode.Unauthorized);

            if(CurrentlyLoggedUser.Role != Role.Administrator)
                return new ServiceResponse<UpdateRaceDtoResponse>(HttpStatusCode.Forbidden);

            if (await Context.Races.Where(x => x.Name == dto.Name).AnyAsync())
                return new ServiceResponse<UpdateRaceDtoResponse>(HttpStatusCode.BadRequest, "Podana rasa już istnieje");

            var race = Context.Races.Find(raceId);
            if (race == null)
                return new ServiceResponse<UpdateRaceDtoResponse>(HttpStatusCode.NotFound);

            var species = Context.Species.Find(dto.SpeciesId);
            if (species == null)
                return new ServiceResponse<UpdateRaceDtoResponse>(HttpStatusCode.BadRequest, "Nie istnieje taki gatunek w bazie danych");

            race.Name = dto.Name;
            race.SpeciesId = dto.SpeciesId;

            var responseDto = Mapper.Map<UpdateRaceDtoResponse>(race);

            return await Context.SaveChangesAsync() > 0
                ? new ServiceResponse<UpdateRaceDtoResponse>(HttpStatusCode.OK, responseDto)
                : new ServiceResponse<UpdateRaceDtoResponse>(HttpStatusCode.BadRequest, "Wystąpił błąd podczas zapisu");
        }

        public async Task<ServiceResponse<DeleteRaceDtoResponse>> DeleteAsync(int raceId)
        {
            if(CurrentlyLoggedUser == null)
                return new ServiceResponse<DeleteRaceDtoResponse>(HttpStatusCode.Unauthorized);

            if(CurrentlyLoggedUser.Role != Role.Administrator)
                return new ServiceResponse<DeleteRaceDtoResponse>(HttpStatusCode.Forbidden);

            var race = Context.Races.Find(raceId);

            if (race == null)
                return new ServiceResponse<DeleteRaceDtoResponse>(HttpStatusCode.NotFound);

            var dto = Mapper.Map<DeleteRaceDtoResponse>(race);
            Context.Races.Remove(race);

            return await Context.SaveChangesAsync() > 0
                ? new ServiceResponse<DeleteRaceDtoResponse>(HttpStatusCode.OK, dto)
                : new ServiceResponse<DeleteRaceDtoResponse>(HttpStatusCode.BadRequest, "Wystąpił błąd podczas zapisu");
        }
    }
}
