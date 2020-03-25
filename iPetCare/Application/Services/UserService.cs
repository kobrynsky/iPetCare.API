﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Application.Dtos.Users;
using Application.Interfaces;
using Application.Services.Utilities;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    public class UserService: Service, IUserService
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IJwtGenerator _jwtGenerator;

        public UserService(IServiceProvider serviceProvider, SignInManager<ApplicationUser> signInManager, IJwtGenerator jwtGenerator) : base(serviceProvider)
        {
            _jwtGenerator = jwtGenerator;
            _signInManager = signInManager;
        }

        public async Task<ServiceResponse<LoginDtoResponse>> LoginAsync(LoginDtoRequest dto)
        {
            var user = await UserManager.FindByEmailAsync(dto.Email);

            if (user == null)
                return new ServiceResponse<LoginDtoResponse>(HttpStatusCode.Unauthorized);

            var result = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, false);

            if (!result.Succeeded)
                return new ServiceResponse<LoginDtoResponse>(HttpStatusCode.Unauthorized);

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

        public async Task<ServiceResponse<RegisterDtoResponse>> RegisterAsync(RegisterDtoRequest dto)
        {
            var validationResponse = await ValidateRegisterRequestAsync(dto);
            if (validationResponse.StatusCode != HttpStatusCode.OK) return validationResponse;

            var userToRegister = new ApplicationUser()
            {
                LastName = dto.LastName,
                FirstName = dto.FirstName,
                Email = dto.Email,
                UserName = dto.UserName,
                Role = dto.Role
            };

            if (dto.Role == Role.Administrator)
                return await RegisterAdminAsync(dto, userToRegister);

            return await RegisterOthersAsync(dto, userToRegister);
        }

        public async Task<ServiceResponse<GetAllDtoResponse>> GetAllAsync()
        {
            if (CurrentlyLoggedUser == null)
                return new ServiceResponse<GetAllDtoResponse>(HttpStatusCode.Unauthorized);

            if (CurrentlyLoggedUser.Role != Role.Administrator)
                return new ServiceResponse<GetAllDtoResponse>(HttpStatusCode.Forbidden);

            var users = await Context.Users.ToListAsync();

            var dto = new GetAllDtoResponse { Users = Mapper.Map<List<UserGetAllDtoResponse>>(users) };
            return new ServiceResponse<GetAllDtoResponse>(HttpStatusCode.OK, dto);
        }

        private async Task<ServiceResponse<RegisterDtoResponse>> RegisterOthersAsync(RegisterDtoRequest dto, ApplicationUser userToRegister)
        {
            if (CurrentlyLoggedUser != null && CurrentlyLoggedUser.Role != Role.Administrator)
                return new ServiceResponse<RegisterDtoResponse>(HttpStatusCode.BadRequest, "Należy się najpierw wylogować");

            var createUserResponse = await CreateUserAsync(dto, userToRegister);
            if (createUserResponse.StatusCode != HttpStatusCode.OK) return createUserResponse;

            var responseDto = new RegisterDtoResponse()
            {
                Id = userToRegister.Id,
                FirstName = userToRegister.FirstName,
                LastName = userToRegister.LastName,
                Email = userToRegister.Email,
                UserName = userToRegister.UserName,
                Role = userToRegister.Role,
                Token = _jwtGenerator.CreateToken(userToRegister)
            };

            if (userToRegister.Role == Role.Vet)
                return await CreateVetAsync(userToRegister, responseDto);

            if (userToRegister.Role == Role.Owner)
                return await CreateOwnerAsync(userToRegister, responseDto);
            return new ServiceResponse<RegisterDtoResponse>(HttpStatusCode.BadRequest, "Podana rola nie istnieje");
        }

        private async Task<ServiceResponse<RegisterDtoResponse>> RegisterAdminAsync(RegisterDtoRequest dto, ApplicationUser userToRegister)
        {
            if (CurrentlyLoggedUser == null)
                return new ServiceResponse<RegisterDtoResponse>(HttpStatusCode.Unauthorized);

            if (CurrentlyLoggedUser.Role != Role.Administrator)
                return new ServiceResponse<RegisterDtoResponse>(HttpStatusCode.Forbidden);

            var createUserResponse = await CreateUserAsync(dto, userToRegister);
            if (createUserResponse.StatusCode != HttpStatusCode.OK) return createUserResponse;

            var responseDto = new RegisterDtoResponse()
            {
                Id = userToRegister.Id,
                FirstName = userToRegister.FirstName,
                LastName = userToRegister.LastName,
                Email = userToRegister.Email,
                UserName = userToRegister.UserName,
                Role = userToRegister.Role,
                Token = _jwtGenerator.CreateToken(userToRegister)
            };
            return new ServiceResponse<RegisterDtoResponse>(HttpStatusCode.OK, responseDto);
        }

        private async Task<ServiceResponse<RegisterDtoResponse>> ValidateRegisterRequestAsync(RegisterDtoRequest dto)
        {
            if (await Context.Users.Where(x => x.Email == dto.Email).AnyAsync())
                return new ServiceResponse<RegisterDtoResponse>(HttpStatusCode.BadRequest, "Email jest zajęty");

            if (await Context.Users.Where(x => x.UserName == dto.UserName).AnyAsync())
                return new ServiceResponse<RegisterDtoResponse>(HttpStatusCode.BadRequest, "Nick jest zajęty");
            return new ServiceResponse<RegisterDtoResponse>(HttpStatusCode.OK);
        }

        private async Task<ServiceResponse<RegisterDtoResponse>> CreateUserAsync(RegisterDtoRequest dto, ApplicationUser userToRegister)
        {
            var result = await UserManager.CreateAsync(userToRegister, dto.Password);
            if (result.Succeeded) return new ServiceResponse<RegisterDtoResponse>(HttpStatusCode.OK);

            var errors = string.Join("\n", result.Errors.Select(x => x.Description));
            return new ServiceResponse<RegisterDtoResponse>(HttpStatusCode.BadRequest, errors);
        }

        private async Task<ServiceResponse<RegisterDtoResponse>> CreateVetAsync(ApplicationUser userToRegister, RegisterDtoResponse responseDto)
        {
            var vet = new Vet
            {
                Id = Guid.Parse(userToRegister.Id),
                User = userToRegister
            };

            Context.Vets.Add(vet);
            return await Context.SaveChangesAsync() <= 0
                ? new ServiceResponse<RegisterDtoResponse>(HttpStatusCode.BadRequest,
                    "Wystąpił błąd podczas dodawania weterynarza")
                : new ServiceResponse<RegisterDtoResponse>(HttpStatusCode.OK, responseDto);
        }

        private async Task<ServiceResponse<RegisterDtoResponse>> CreateOwnerAsync(ApplicationUser userToRegister, RegisterDtoResponse responseDto)
        {
            var owner = new Owner
            {
                Id = Guid.Parse(userToRegister.Id),
                User = userToRegister
            };
            Context.Owners.Add(owner);
            return await Context.SaveChangesAsync() <= 0
                ? new ServiceResponse<RegisterDtoResponse>(HttpStatusCode.BadRequest,
                    "Wystąpił błąd podczas dodawania opiekuna")
                : new ServiceResponse<RegisterDtoResponse>(HttpStatusCode.OK, responseDto);
        }
    }
}
