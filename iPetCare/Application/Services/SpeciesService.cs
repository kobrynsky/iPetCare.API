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


        public async Task<ServiceResponse<SpeciesCreateDtoResponse>> CreateAsync(SpeciesCreateDtoRequest dto)
        {
            if (await _context.Species.Where(x => x.Name == dto.Name).AnyAsync())
                return new ServiceResponse<SpeciesCreateDtoResponse>(HttpStatusCode.BadRequest, "Podany gatunek już istnieje");

            var currentUserName = _userAccessor.GetCurrentUsername();

            if (currentUserName != null)
            {
                var currentUser = await _userManager.FindByNameAsync(currentUserName);
                if (currentUser != null && currentUser.Role != Role.Administrator || currentUser == null)
                    return new ServiceResponse<SpeciesCreateDtoResponse>(HttpStatusCode.Forbidden, "Brak uprawnień");
            }
            else
            {
                return new ServiceResponse<SpeciesCreateDtoResponse>(HttpStatusCode.Unauthorized, "Brak uprawnień");
            }

            var species = new Species()
            {
                Name = dto.Name
            };

            _context.Species.Add(species);
            int result = await _context.SaveChangesAsync();

            if (result > 0)
            {
                var responseDto = new SpeciesCreateDtoResponse()
                {
                    Name = species.Name,
                    Id = species.Id
                };

                return new ServiceResponse<SpeciesCreateDtoResponse>(HttpStatusCode.OK, responseDto);
            }

            return new ServiceResponse<SpeciesCreateDtoResponse>(HttpStatusCode.BadRequest);
        }

        public async Task<ServiceResponse<SpeciesDeleteDtoResponse>> DeleteAsync(int speciesId)
        {
            var currentUserName = _userAccessor.GetCurrentUsername();

            if (currentUserName == null)
                return new ServiceResponse<SpeciesDeleteDtoResponse>(HttpStatusCode.Unauthorized, "Brak uprawnień");

            var currentUser = await _userManager.FindByNameAsync(currentUserName);
            if (currentUser != null && currentUser.Role != Role.Administrator)
                return new ServiceResponse<SpeciesDeleteDtoResponse>(HttpStatusCode.Forbidden, "Brak uprawnień");

            var species = _context.Species.Find(speciesId);

            if (species == null)
                return new ServiceResponse<SpeciesDeleteDtoResponse>(HttpStatusCode.BadRequest, "Nie istnieje taka rasa w bazie danych");

            var dto = _mapper.Map<SpeciesDeleteDtoResponse>(species);

            _context.Species.Remove(species);
            int result = await _context.SaveChangesAsync();

            if (result > 0)
                return new ServiceResponse<SpeciesDeleteDtoResponse>(HttpStatusCode.OK, dto);

            return new ServiceResponse<SpeciesDeleteDtoResponse>(HttpStatusCode.BadRequest);
        }

        public async Task<ServiceResponse<SpeciesGetAllDtoResponse>> GetAllAsync()
        {
            var currentUserName = _userAccessor.GetCurrentUsername();

            if (currentUserName == null)
                return new ServiceResponse<SpeciesGetAllDtoResponse>(HttpStatusCode.Unauthorized, "Brak uprawnień");

            var currentUser = await _userManager.FindByNameAsync(currentUserName);
            if (currentUser == null)
                return new ServiceResponse<SpeciesGetAllDtoResponse>(HttpStatusCode.Unauthorized, "Brak uprawnień");

            var species = await _context.Species.ToListAsync();

            var dto = new SpeciesGetAllDtoResponse();
            dto.Species = _mapper.Map<List<SpeciesDetailGetAllDtoResponse>>(species);

            return new ServiceResponse<SpeciesGetAllDtoResponse>(HttpStatusCode.OK, dto);
        }

        public async Task<ServiceResponse<SpeciesGetDtoResponse>> GetAsync(int speciesId)
        {
            var currentUserName = _userAccessor.GetCurrentUsername();

            if (currentUserName == null)
                return new ServiceResponse<SpeciesGetDtoResponse>(HttpStatusCode.Unauthorized, "Brak uprawnień");

            var currentUser = await _userManager.FindByNameAsync(currentUserName);
            if (currentUser == null)
                return new ServiceResponse<SpeciesGetDtoResponse>(HttpStatusCode.Unauthorized, "Brak uprawnień");

            var species = await _context.Species.FindAsync(speciesId);

            if (species == null)
                return new ServiceResponse<SpeciesGetDtoResponse>(HttpStatusCode.BadRequest, "Nie istnieje taka rasa w bazie danych");

            var dto = _mapper.Map<SpeciesGetDtoResponse>(species);

            var races = await _context.Races.ToListAsync();

            if (races != null)
                dto.Races = _mapper.Map<List<RaceDetailsGetDtoResponse>>(races);

            return new ServiceResponse<SpeciesGetDtoResponse>(HttpStatusCode.OK, dto);
        }

        public async Task<ServiceResponse<SpeciesUpdateDtoResponse>> UpdateAsync(int speciesId, SpeciesUpdateDtoRequest dto)
        {
            var currentUserName = _userAccessor.GetCurrentUsername();

            if (currentUserName == null)
                return new ServiceResponse<SpeciesUpdateDtoResponse>(HttpStatusCode.Unauthorized, "Brak uprawnień");

            var currentUser = await _userManager.FindByNameAsync(currentUserName);
            if (currentUser != null && currentUser.Role != Role.Administrator)
                return new ServiceResponse<SpeciesUpdateDtoResponse>(HttpStatusCode.Forbidden, "Brak uprawnień");

            if (await _context.Species.Where(x => x.Name == dto.Name).AnyAsync())
                return new ServiceResponse<SpeciesUpdateDtoResponse>(HttpStatusCode.BadRequest, "Podana rasa już istnieje");

            var species = _context.Species.Find(speciesId);

            if (species == null)
                return new ServiceResponse<SpeciesUpdateDtoResponse>(HttpStatusCode.BadRequest, "Nie istnieje taki gatunek w bazie danych");

            species.Name = dto.Name;
            int result = await _context.SaveChangesAsync();

            if (result > 0)
            {
                var responseDto = _mapper.Map<SpeciesUpdateDtoResponse>(species);
                return new ServiceResponse<SpeciesUpdateDtoResponse>(HttpStatusCode.OK, responseDto);

            }

            if (result == 0)
                return new ServiceResponse<SpeciesUpdateDtoResponse>(HttpStatusCode.BadRequest, "Nie nastąpiła żadna zmiana");

            return new ServiceResponse<SpeciesUpdateDtoResponse>(HttpStatusCode.BadRequest);
        }
    }
}
