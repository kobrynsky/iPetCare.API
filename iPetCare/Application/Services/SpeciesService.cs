using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Application.Dtos.Species;
using Application.Interfaces;
using Application.Services.Utilities;
using AutoMapper;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Services
{
    public class SpeciesService : ISpeciesService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly DataContext _context;
        private readonly IUserAccessor _userAccessor;
        private readonly IMapper _mapper;

        public SpeciesService(UserManager<ApplicationUser> userManager, DataContext context, IUserAccessor userAccessor, IMapper mapper)
        {
            _context = context;
            _userAccessor = userAccessor;
            _userManager = userManager;
            _mapper = mapper;
        }


        public async Task<ServiceResponse<SpeciesCreateSpeciesDtoResponse>> CreateAsync(SpeciesCreateSpeciesDtoRequest dto)
        {
            if (await _context.Species.Where(x => x.Name == dto.Name).AnyAsync())
                return new ServiceResponse<SpeciesCreateSpeciesDtoResponse>(HttpStatusCode.BadRequest, "Podany gatunek już istnieje");

            var currentUserName = _userAccessor.GetCurrentUsername();

            if (currentUserName != null)
            {
                var currentUser = await _userManager.FindByNameAsync(currentUserName);
                if (currentUser != null && currentUser.Role != Role.Administrator || currentUser == null)
                    return new ServiceResponse<SpeciesCreateSpeciesDtoResponse>(HttpStatusCode.Forbidden, "Brak uprawnień");
            }
            else
            {
                return new ServiceResponse<SpeciesCreateSpeciesDtoResponse>(HttpStatusCode.Unauthorized, "Brak uprawnień");
            }

            var species = new Species()
            {
                Name = dto.Name
            };

            _context.Species.Add(species);
            int result = await _context.SaveChangesAsync();

            if (result > 0)
            {
                var responseDto = new SpeciesCreateSpeciesDtoResponse()
                {
                    Name = species.Name,
                    Id = species.Id
                };

                return new ServiceResponse<SpeciesCreateSpeciesDtoResponse>(HttpStatusCode.OK, responseDto);
            }

            return new ServiceResponse<SpeciesCreateSpeciesDtoResponse>(HttpStatusCode.BadRequest);
        }

        public async Task<ServiceResponse<SpeciesDeleteSpeciesDtoResponse>> DeleteAsync(int speciesId)
        {
            var currentUserName = _userAccessor.GetCurrentUsername();

            if (currentUserName == null)
                return new ServiceResponse<SpeciesDeleteSpeciesDtoResponse>(HttpStatusCode.Unauthorized, "Brak uprawnień");

            var currentUser = await _userManager.FindByNameAsync(currentUserName);
            if (currentUser != null && currentUser.Role != Role.Administrator)
                return new ServiceResponse<SpeciesDeleteSpeciesDtoResponse>(HttpStatusCode.Forbidden, "Brak uprawnień");

            var species = _context.Species.Find(speciesId);

            if (species == null)
                return new ServiceResponse<SpeciesDeleteSpeciesDtoResponse>(HttpStatusCode.BadRequest, "Nie istnieje taka rasa w bazie danych");

            var dto = _mapper.Map<SpeciesDeleteSpeciesDtoResponse>(species);

            _context.Species.Remove(species);
            int result = await _context.SaveChangesAsync();

            if (result > 0)
                return new ServiceResponse<SpeciesDeleteSpeciesDtoResponse>(HttpStatusCode.OK, dto);

            return new ServiceResponse<SpeciesDeleteSpeciesDtoResponse>(HttpStatusCode.BadRequest);
        }

        public async Task<ServiceResponse<SpeciesGetAllSpeciesDtoResponse>> GetAllAsync()
        {
            var currentUserName = _userAccessor.GetCurrentUsername();

            if (currentUserName == null)
                return new ServiceResponse<SpeciesGetAllSpeciesDtoResponse>(HttpStatusCode.Unauthorized, "Brak uprawnień");

            var currentUser = await _userManager.FindByNameAsync(currentUserName);
            if (currentUser == null)
                return new ServiceResponse<SpeciesGetAllSpeciesDtoResponse>(HttpStatusCode.Unauthorized, "Brak uprawnień");

            var species = await _context.Species.ToListAsync();

            var dto = new SpeciesGetAllSpeciesDtoResponse();
            dto.Species = _mapper.Map<List<SpeciesDetailGetAllDtoResponse>>(species);

            return new ServiceResponse<SpeciesGetAllSpeciesDtoResponse>(HttpStatusCode.OK, dto);
        }

        public async Task<ServiceResponse<SpeciesGetSpeciesDtoResponse>> GetAsync(int speciesId)
        {
            var currentUserName = _userAccessor.GetCurrentUsername();

            if (currentUserName == null)
                return new ServiceResponse<SpeciesGetSpeciesDtoResponse>(HttpStatusCode.Unauthorized, "Brak uprawnień");

            var currentUser = await _userManager.FindByNameAsync(currentUserName);
            if (currentUser == null)
                return new ServiceResponse<SpeciesGetSpeciesDtoResponse>(HttpStatusCode.Unauthorized, "Brak uprawnień");

            var species = await _context.Species.FindAsync(speciesId);

            if (species == null)
                return new ServiceResponse<SpeciesGetSpeciesDtoResponse>(HttpStatusCode.BadRequest, "Nie istnieje taka rasa w bazie danych");

            var dto = _mapper.Map<SpeciesGetSpeciesDtoResponse>(species);

            var races = await _context.Races.ToListAsync();

            var filteredRaces = races.Where(race => race.SpeciesId == speciesId).ToList();

            if (filteredRaces != null)
                dto.Races = _mapper.Map<List<RaceDetailsGetDtoResponse>>(filteredRaces);

            return new ServiceResponse<SpeciesGetSpeciesDtoResponse>(HttpStatusCode.OK, dto);
        }

        public async Task<ServiceResponse<SpeciesUpdateSpeciesDtoResponse>> UpdateAsync(int speciesId, SpeciesUpdateSpeciesDtoRequest dto)
        {
            var currentUserName = _userAccessor.GetCurrentUsername();

            if (currentUserName == null)
                return new ServiceResponse<SpeciesUpdateSpeciesDtoResponse>(HttpStatusCode.Unauthorized, "Brak uprawnień");

            var currentUser = await _userManager.FindByNameAsync(currentUserName);
            if (currentUser != null && currentUser.Role != Role.Administrator || currentUser == null)
                return new ServiceResponse<SpeciesUpdateSpeciesDtoResponse>(HttpStatusCode.Forbidden, "Brak uprawnień");

            if (await _context.Species.Where(x => x.Name == dto.Name).AnyAsync())
                return new ServiceResponse<SpeciesUpdateSpeciesDtoResponse>(HttpStatusCode.BadRequest, "Podana rasa już istnieje");

            var species = _context.Species.Find(speciesId);

            if (species == null)
                return new ServiceResponse<SpeciesUpdateSpeciesDtoResponse>(HttpStatusCode.BadRequest, "Nie istnieje taki gatunek w bazie danych");

            if (species.Name.Equals(dto.Name))
            {
                var responseDto = _mapper.Map<SpeciesUpdateSpeciesDtoResponse>(species);
                return new ServiceResponse<SpeciesUpdateSpeciesDtoResponse>(HttpStatusCode.OK, responseDto);
            }

            species.Name = dto.Name;
            int result = await _context.SaveChangesAsync();

            if (result > 0)
            {
                var responseDto = _mapper.Map<SpeciesUpdateSpeciesDtoResponse>(species);
                return new ServiceResponse<SpeciesUpdateSpeciesDtoResponse>(HttpStatusCode.OK, responseDto);
            }

            return new ServiceResponse<SpeciesUpdateSpeciesDtoResponse>(HttpStatusCode.BadRequest);
        }
    }
}
