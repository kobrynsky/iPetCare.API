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
        public async Task<ServiceResponse<CreateDtoResponse>> CreateAsync(CreateDtoRequest dto)
        {
            if (await _context.Races.Where(x => x.Name == dto.Name).AnyAsync())
                return new ServiceResponse<CreateDtoResponse>(HttpStatusCode.BadRequest, "Podana rasa już istnieje");

            var currentUserName = _userAccessor.GetCurrentUsername();

            if (currentUserName != null)
            {
                var currentUser = await _userManager.FindByNameAsync(currentUserName);
                if (currentUser != null && currentUser.Role != Role.Administrator)
                    return new ServiceResponse<CreateDtoResponse>(HttpStatusCode.Forbidden, "Brak uprawnień");
            }

            if (currentUserName == null)
            {
                return new ServiceResponse<CreateDtoResponse>(HttpStatusCode.Unauthorized, "Brak uprawnień");
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
                var responseDto = new CreateDtoResponse()
                {
                    Name = race.Name,
                    SpeciesId = race.SpeciesId,
                    Id = race.Id
                };

                return new ServiceResponse<CreateDtoResponse>(HttpStatusCode.OK, responseDto);
            }

            return new ServiceResponse<CreateDtoResponse>(HttpStatusCode.BadRequest);
        }

        public async Task<ServiceResponse<GetAllDtoResponse>> GetAllAsync()
        {
            var currentUserName = _userAccessor.GetCurrentUsername();

            if (currentUserName == null)
                return new ServiceResponse<GetAllDtoResponse>(HttpStatusCode.Unauthorized, "Brak uprawnień");

            var currentUser = await _userManager.FindByNameAsync(currentUserName);
            if (currentUser == null)
                return new ServiceResponse<GetAllDtoResponse>(HttpStatusCode.Unauthorized, "Brak uprawnień");

            var races = await _context.Races.ToListAsync();

            var dto = new GetAllDtoResponse();
            dto.Races = _mapper.Map<List<RaceGetAllDtoResponse>>(races);

            return new ServiceResponse<GetAllDtoResponse>(HttpStatusCode.OK, dto);
        }

        public async Task<ServiceResponse<GetDtoResponse>> GetAsync(int raceId)
        {
            var currentUserName = _userAccessor.GetCurrentUsername();

            if (currentUserName == null)
                return new ServiceResponse<GetDtoResponse>(HttpStatusCode.Unauthorized, "Brak uprawnień");

            var currentUser = await _userManager.FindByNameAsync(currentUserName);
            if (currentUser == null)
                return new ServiceResponse<GetDtoResponse>(HttpStatusCode.Unauthorized, "Brak uprawnień");

            var race = _context.Races.Find(raceId);

            var dto = _mapper.Map<GetDtoResponse>(race);

            return new ServiceResponse<GetDtoResponse>(HttpStatusCode.OK, dto);
        }
    }
}
