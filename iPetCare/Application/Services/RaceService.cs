using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Application.Dtos.Races;
using Application.Interfaces;
using Application.Services.Utilities;
using AutoMapper;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Services
{
    public class RaceService : IRaceService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly DataContext _context;
        private readonly IUserAccessor _userAccessor;
        private readonly IMapper _mapper;

        public RaceService(UserManager<ApplicationUser> userManager, DataContext context, IUserAccessor userAccessor, IMapper mapper)
        {
            _context = context;
            _userAccessor = userAccessor;
            _userManager = userManager;
            _mapper = mapper;
        }
        public async Task<ServiceResponse<RaceCreateDtoResponse>> CreateAsync(RaceCreateDtoRequest dto)
        {
            if (await _context.Races.Where(x => x.Name == dto.Name).AnyAsync())
                return new ServiceResponse<RaceCreateDtoResponse>(HttpStatusCode.BadRequest, "Podana rasa już istnieje");

            var currentUserName = _userAccessor.GetCurrentUsername();

            if (currentUserName != null)
            {
                var currentUser = await _userManager.FindByNameAsync(currentUserName);

                if (currentUser == null)
                    return new ServiceResponse<RaceCreateDtoResponse>(HttpStatusCode.Unauthorized, "Brak uprawnień");

                if (currentUser != null && currentUser.Role != Role.Administrator)
                    return new ServiceResponse<RaceCreateDtoResponse>(HttpStatusCode.Forbidden, "Brak uprawnień");
            }

            if (currentUserName == null)
            {
                return new ServiceResponse<RaceCreateDtoResponse>(HttpStatusCode.Unauthorized, "Brak uprawnień");
            }

            var race = new Race()
            {
                Name = dto.Name,
                SpeciesId = dto.SpeciesId
            };

            _context.Races.Add(race);
            int result = await _context.SaveChangesAsync();

            if(result > 0)
            {
                var responseDto = new RaceCreateDtoResponse()
                {
                    Name = race.Name,
                    SpeciesId = race.SpeciesId,
                    Id = race.Id
                };

                return new ServiceResponse<RaceCreateDtoResponse>(HttpStatusCode.OK, responseDto);
            }

            return new ServiceResponse<RaceCreateDtoResponse>(HttpStatusCode.BadRequest);
        }

        public async Task<ServiceResponse<RaceGetAllDtoResponse>> GetAllAsync()
        {
            var currentUserName = _userAccessor.GetCurrentUsername();

            if (currentUserName == null)
                return new ServiceResponse<RaceGetAllDtoResponse>(HttpStatusCode.Unauthorized, "Brak uprawnień");

            var currentUser = await _userManager.FindByNameAsync(currentUserName);
            if (currentUser == null)
                return new ServiceResponse<RaceGetAllDtoResponse>(HttpStatusCode.Unauthorized, "Brak uprawnień");

            var races = await _context.Races.ToListAsync();

            var dto = new RaceGetAllDtoResponse();
            dto.Races = _mapper.Map<List<RaceDetailGetAllDtoResponse>>(races);

            return new ServiceResponse<RaceGetAllDtoResponse>(HttpStatusCode.OK, dto);
        }

        public async Task<ServiceResponse<RaceGetDtoResponse>> GetAsync(int raceId)
        {
            var currentUserName = _userAccessor.GetCurrentUsername();

            if (currentUserName == null)
                return new ServiceResponse<RaceGetDtoResponse>(HttpStatusCode.Unauthorized, "Brak uprawnień");

            var currentUser = await _userManager.FindByNameAsync(currentUserName);
            if (currentUser == null)
                return new ServiceResponse<RaceGetDtoResponse>(HttpStatusCode.Unauthorized, "Brak uprawnień");

            var race = await _context.Races.FindAsync(raceId);
    
            if (race == null)
                return new ServiceResponse<RaceGetDtoResponse>(HttpStatusCode.BadRequest, "Nie istnieje taka rasa w bazie danych");

            var dto = _mapper.Map<RaceGetDtoResponse>(race);

            return new ServiceResponse<RaceGetDtoResponse>(HttpStatusCode.OK, dto);
        }

        public async Task<ServiceResponse<RaceUpdateDtoResponse>> UpdateAsync(int raceId, RaceUpdateDtoRequest dto)
        {
            var currentUserName = _userAccessor.GetCurrentUsername();

            if (currentUserName == null)
                return new ServiceResponse<RaceUpdateDtoResponse>(HttpStatusCode.Unauthorized, "Brak uprawnień");

            var currentUser = await _userManager.FindByNameAsync(currentUserName);
            if (currentUser != null && currentUser.Role != Role.Administrator)
                return new ServiceResponse<RaceUpdateDtoResponse>(HttpStatusCode.Forbidden, "Brak uprawnień");

            if (await _context.Races.Where(x => x.Name == dto.Name).AnyAsync())
                return new ServiceResponse<RaceUpdateDtoResponse>(HttpStatusCode.BadRequest, "Podana rasa już istnieje");
            
            var race = _context.Races.Find(raceId);

            var species = _context.Species.Find(dto.SpeciesId);

            if (species == null)
                return new ServiceResponse<RaceUpdateDtoResponse>(HttpStatusCode.BadRequest, "Nie istnieje taki gatunek w bazie danych");

            if (race == null)
                return new ServiceResponse<RaceUpdateDtoResponse>(HttpStatusCode.BadRequest, "Nie istnieje taka rasa w bazie danych");

            race.Name = dto.Name;
            race.SpeciesId = dto.SpeciesId;

            int result = await _context.SaveChangesAsync();

            if (result > 0)
            {
                var responseDto = _mapper.Map<RaceUpdateDtoResponse>(race);
                return new ServiceResponse<RaceUpdateDtoResponse>(HttpStatusCode.OK, responseDto);

            }

            if (result == 0)
                return new ServiceResponse<RaceUpdateDtoResponse>(HttpStatusCode.BadRequest, "Nie nastąpiła żadna zmiana");

            return new ServiceResponse<RaceUpdateDtoResponse>(HttpStatusCode.BadRequest);
        }

        public async Task<ServiceResponse<RaceDeleteDtoResponse>> DeleteAsync(int raceId)
        {
            var currentUserName = _userAccessor.GetCurrentUsername();

            if (currentUserName == null)
                return new ServiceResponse<RaceDeleteDtoResponse>(HttpStatusCode.Unauthorized, "Brak uprawnień");

            var currentUser = await _userManager.FindByNameAsync(currentUserName);
            if (currentUser != null && currentUser.Role != Role.Administrator)
                return new ServiceResponse<RaceDeleteDtoResponse>(HttpStatusCode.Forbidden, "Brak uprawnień");

            var race = _context.Races.Find(raceId);

            if (race == null)
                return new ServiceResponse<RaceDeleteDtoResponse>(HttpStatusCode.BadRequest, "Nie istnieje taka rasa w bazie danych");

            var dto = _mapper.Map<RaceDeleteDtoResponse>(race);

            _context.Races.Remove(race);
            int result = await _context.SaveChangesAsync();

            if (result > 0)
                return new ServiceResponse<RaceDeleteDtoResponse>(HttpStatusCode.OK, dto);
            
            return new ServiceResponse<RaceDeleteDtoResponse>(HttpStatusCode.BadRequest);
        }
    }
}
