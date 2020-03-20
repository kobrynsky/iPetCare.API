using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Application.Dtos.Users;
using Application.Interfaces;
using Application.Services.Utilities;
using AutoMapper;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Services
{
    public class UserService: IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IJwtGenerator _jwtGenerator;
        private readonly DataContext _context;
        private readonly IUserAccessor _userAccessor;
        private readonly IMapper _mapper;

        public UserService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IJwtGenerator jwtGenerator, DataContext context, IUserAccessor userAccessor, IMapper mapper)
        {
            _jwtGenerator = jwtGenerator;
            _context = context;
            _userAccessor = userAccessor;
            _mapper = mapper;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public async Task<ServiceResponse<LoginDtoResponse>> LoginAsync(LoginDtoRequest dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);

            if (user == null)
                return new ServiceResponse<LoginDtoResponse>(HttpStatusCode.Unauthorized);

            var result = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, false);

            if (result.Succeeded)
            {
                var responseDto = new LoginDtoResponse()
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    UserName = user.UserName,
                    Email = user.Email,
                    Token = _jwtGenerator.CreateToken(user),
                    Role = user.Role,
                };

                return new ServiceResponse<LoginDtoResponse>(HttpStatusCode.OK, responseDto);
            }

            return new ServiceResponse<LoginDtoResponse>(HttpStatusCode.Unauthorized);
        }

        public async Task<ServiceResponse<RegisterDtoResponse>> RegisterAsync(RegisterDtoRequest dto)
        {
            if (await _context.Users.Where(x => x.Email == dto.Email).AnyAsync())
                return new ServiceResponse<RegisterDtoResponse>(HttpStatusCode.BadRequest, "Email jest zajęty");

            if (await _context.Users.Where(x => x.UserName == dto.UserName).AnyAsync())
                return new ServiceResponse<RegisterDtoResponse>(HttpStatusCode.BadRequest, "Nick jest zajęty");

            var currentUserName = _userAccessor.GetCurrentUsername();

            if (currentUserName != null)
            {
                var currentUser = await _userManager.FindByNameAsync(currentUserName);
                if (currentUser != null && currentUser.Role != Role.Administrator)
                    return new ServiceResponse<RegisterDtoResponse>(HttpStatusCode.Forbidden, "Brak uprawnień do rejestracji konta z tą rolą");
            }


            if (dto.Role == Role.Administrator)
            {
                var currentUser = await _userManager.FindByNameAsync(currentUserName);
                if (currentUser == null)
                    return new ServiceResponse<RegisterDtoResponse>(HttpStatusCode.Unauthorized, "Brak uprawnień do rejestracji konta z tą rolą");
                if(currentUser.Role != Role.Administrator)
                    return new ServiceResponse<RegisterDtoResponse>(HttpStatusCode.Forbidden, "Brak uprawnień do rejestracji konta z tą rolą");
            }

            if (dto.Role == Role.Owner || dto.Role == Role.Vet)
            {
                var user = new ApplicationUser()
                {
                    LastName = dto.LastName,
                    FirstName = dto.FirstName,
                    Email = dto.Email,
                    UserName = dto.UserName,
                    Role = dto.Role
                };

                var result = await _userManager.CreateAsync(user, dto.Password);

                if (result.Succeeded)
                {
                    var responseDto = new RegisterDtoResponse()
                    {
                        Id = user.Id,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.Email,
                        UserName = user.UserName,
                        Role = user.Role,
                        Token = _jwtGenerator.CreateToken(user)
                    };

                    // assign to vet or owner table here
                    if (user.Role == Role.Vet)
                    {
                        var vet = new Vet
                        {
                            Id = Guid.Parse(user.Id),
                            User = user
                        };

                        _context.Vets.Add(vet);
                        if(await _context.SaveChangesAsync() <= 0)
                            return new ServiceResponse<RegisterDtoResponse>(HttpStatusCode.BadRequest, "An error occured while creating vet");
                    }
                    else
                    {
                        var owner = new Owner
                        {
                            Id = Guid.Parse(user.Id),
                            User = user
                        };

                        _context.Owners.Add(owner);
                        if (await _context.SaveChangesAsync() <= 0)
                            return new ServiceResponse<RegisterDtoResponse>(HttpStatusCode.BadRequest, "An error occured while creating owner");
                    }

                    return new ServiceResponse<RegisterDtoResponse>(HttpStatusCode.OK, responseDto);
                };
                var errors = string.Join("\n", result.Errors.Select(x => x.Description));
                return new ServiceResponse<RegisterDtoResponse>(HttpStatusCode.BadRequest, errors);
            }
            else
                return new ServiceResponse<RegisterDtoResponse>(HttpStatusCode.BadRequest, "Podana rola nie istnieje");
        }

        public async Task<ServiceResponse<GetAllDtoResponse>> GetAllAsync()
        {
            var currentUserName = _userAccessor.GetCurrentUsername();

            if (currentUserName == null)
                return new ServiceResponse<GetAllDtoResponse>(HttpStatusCode.Unauthorized, "Brak uprawnień");

            var currentUser = await _userManager.FindByNameAsync(currentUserName);
            if (currentUser == null)
                return new ServiceResponse<GetAllDtoResponse>(HttpStatusCode.Unauthorized, "Brak uprawnień");
            if(currentUser.Role != Role.Administrator)
                return new ServiceResponse<GetAllDtoResponse>(HttpStatusCode.Forbidden, "Brak uprawnień");

            var users = await _context.Users.ToListAsync();

            var dto = new GetAllDtoResponse();
            dto.Users = _mapper.Map<List<UserGetAllDtoResponse>>(users);

            return new ServiceResponse<GetAllDtoResponse>(HttpStatusCode.OK, dto);
        }
    }
}
