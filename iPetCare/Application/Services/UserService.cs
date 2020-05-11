using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Application.Dtos.Owners;
using Application.Dtos.Users;
using Application.Dtos.Vets;
using Application.Interfaces;
using Application.Services.Utilities;
using Domain.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence.Enums;

namespace Application.Services
{
    public class UserService: Service, IUserService
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IJwtGenerator _jwtGenerator;
        private readonly IHostingEnvironment _hostingEnvironment;

        public UserService(IServiceProvider serviceProvider, SignInManager<ApplicationUser> signInManager, IJwtGenerator jwtGenerator, IHostingEnvironment hostingEnvironment) : base(serviceProvider)
        {
            _jwtGenerator = jwtGenerator;
            _hostingEnvironment = hostingEnvironment;
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
                ImageUrl = user.ImageUrl
            };

            if (user.Owner != null)
                responseDto.PlaceOfResidence = user.Owner.PlaceOfResidence;
            else if (user.Vet != null)
                responseDto.Specialization = user.Vet.Specialization;

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

        public async Task<ServiceResponse<GetAllUsersDtoResponse>> GetAllAsync()
        {
            if (CurrentlyLoggedUser == null)
                return new ServiceResponse<GetAllUsersDtoResponse>(HttpStatusCode.Unauthorized);

            if (CurrentlyLoggedUser.Role != Role.Administrator)
                return new ServiceResponse<GetAllUsersDtoResponse>(HttpStatusCode.Forbidden);

            var users = await Context.Users.ToListAsync();

            var dto = new GetAllUsersDtoResponse { Users = Mapper.Map<List<UserForGetAllUsersDtoResponse>>(users) };
            return new ServiceResponse<GetAllUsersDtoResponse>(HttpStatusCode.OK, dto);
        }

        public async Task<ServiceResponse<EditProfileDtoResponse>> EditProfileAsync(EditProfileDtoRequest dto)
        {
            if (CurrentlyLoggedUser == null)
                return new ServiceResponse<EditProfileDtoResponse>(HttpStatusCode.Unauthorized);

            if (CurrentlyLoggedUser.Role == Role.Owner)
                return await EditOwnerProfileAsync(dto);

            if (CurrentlyLoggedUser.Role == Role.Vet)
                return await EditVetProfileAsync(dto);

            if (CurrentlyLoggedUser.Role == Role.Administrator)
                return await EditAdminProfileAsync(dto);

            return new ServiceResponse<EditProfileDtoResponse>(HttpStatusCode.BadRequest);
        }

        private async Task<bool> ChangeUserImageAsync(ApplicationUser currentlyLoggedUser, IFormFile image)
        {
            if (image == null || image.Length <= 0) return false;

            var fileExtension = Path.GetExtension(image.FileName);

            var imageFolderPath = "Uploads/Users/Photos";

            // create folder it should upload files to
            Directory.CreateDirectory($"{_hostingEnvironment.WebRootPath}/{imageFolderPath}");

            // find images named as pet id, regardless of the extension
            var files = Directory.GetFiles($"{_hostingEnvironment.WebRootPath}/{imageFolderPath}", $"{currentlyLoggedUser.Id}.*");

            // if found any files
            if (files.Length > 0)
                // delete them
                foreach (var file in files)
                    File.Delete(file);

            var newFileName = $"{currentlyLoggedUser.Id}{fileExtension}";

            var filePath = Path.Combine(_hostingEnvironment.WebRootPath, imageFolderPath, newFileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }

            currentlyLoggedUser.ImageUrl = $"/{imageFolderPath}/{newFileName}";

            return true;
        }

        public async Task<ServiceResponse<GetVetsDtoResponse>> GetVetsAsync(GetVetsDtoRequest dto)
        {
            int page;
            int pageSize;
            GetVetsSortBy sortBy;

            if (dto.Page == null || dto.Page == 0)
                page = 1;
            else
                page = (int) dto.Page;

            if (dto.PageSize == null || dto.PageSize < 10)
                pageSize = 10;
            else
                pageSize = (int) dto.PageSize;

            if (dto.SortBy == null)
                sortBy = GetVetsSortBy.SortByLastNameAsc;
            else
                sortBy = (GetVetsSortBy) dto.SortBy;

            IQueryable<Vet> dbQuery;

            if (string.IsNullOrWhiteSpace(dto.Query))
                dbQuery = Context.Vets;
            else
            {
                string queryLower = dto.Query.ToLower();
                dbQuery = Context.Vets
                    .Where(v => v.User.LastName.ToLower().Contains(queryLower)
                                || v.User.FirstName.ToLower().Contains(queryLower)
                                || v.InstitutionVets.Any(iv => iv.Institution.Name.ToLower().Contains(queryLower))
                                || v.Specialization.ToLower().Contains(queryLower)
                                || v.InstitutionVets.Any(iv => iv.Institution.Address.ToLower().Contains(queryLower)));
            }

            int totalItems = await dbQuery.CountAsync();

            dbQuery = sortBy switch
            {
                GetVetsSortBy.SortByLastNameAsc => dbQuery.OrderBy(v => v.User.LastName),
                GetVetsSortBy.SortByLastNameDesc => dbQuery.OrderByDescending(v => v.User.LastName),
                GetVetsSortBy.SortBySpecializationAsc => dbQuery.OrderBy(v => v.Specialization),
                GetVetsSortBy.SortBySpecializationDesc => dbQuery.OrderByDescending(v => v.Specialization),
                _ => dbQuery.OrderBy(v => v.User.LastName)
            };
            var vets = await dbQuery.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            var dtoToReturn = new GetVetsDtoResponse
            {
                Query = dto.Query,
                PageSize = pageSize,
                Page = page,
                SortBy = sortBy,
                TotalItems = totalItems,
                Vets = Mapper.Map<List<VetForGetVetsDto>>(vets),
                CurrentSearchingUserRole = CurrentlyLoggedUser.Role
            };

            return new ServiceResponse<GetVetsDtoResponse>(HttpStatusCode.OK, dtoToReturn);
        }

        public async Task<ServiceResponse<GetOwnersDtoResponse>> GetOwnersAsync(GetOwnersDtoRequest dto)
        {
            int page;
            int pageSize;
            GetOwnersSortBy sortBy;

            if (dto.Page == null || dto.Page == 0)
                page = 1;
            else
                page = (int)dto.Page;

            if (dto.PageSize == null || dto.PageSize < 10)
                pageSize = 10;
            else
                pageSize = (int)dto.PageSize;

            if (dto.SortBy == null)
                sortBy = GetOwnersSortBy.SortByLastNameAsc;
            else
                sortBy = (GetOwnersSortBy)dto.SortBy;

            IQueryable<Owner> dbQuery;

            if (string.IsNullOrWhiteSpace(dto.Query))
                dbQuery = Context.Owners;
            else
            {
                string queryLower = dto.Query.ToLower();
                dbQuery = Context.Owners
                    .Where(o => o.User.LastName.ToLower().Contains(queryLower)
                                || o.User.FirstName.ToLower().Contains(queryLower)
                                || o.PlaceOfResidence.ToLower().Contains(queryLower));
            }

            int totalItems = await dbQuery.CountAsync();

            dbQuery = sortBy switch
            {
                GetOwnersSortBy.SortByLastNameAsc => dbQuery.OrderBy(o => o.User.LastName),
                GetOwnersSortBy.SortByLastNameDesc => dbQuery.OrderByDescending(o => o.User.LastName),
                GetOwnersSortBy.SortByPlaceOfResidenceAsc => dbQuery.OrderBy(o => o.PlaceOfResidence),
                GetOwnersSortBy.SortByPlaceOfResidenceDesc => dbQuery.OrderByDescending(o => o.PlaceOfResidence),
                _ => dbQuery.OrderBy(o => o.User.LastName)
            };
            var owners = await dbQuery.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            var dtoToReturn = new GetOwnersDtoResponse
            {
                Query = dto.Query,
                PageSize = pageSize,
                Page = page,
                SortBy = sortBy,
                TotalItems = totalItems,
                Owners = Mapper.Map<List<OwnerForGetOwnersDto>>(owners),
                CurrentSearchingUserRole = CurrentlyLoggedUser.Role
            };

            return new ServiceResponse<GetOwnersDtoResponse>(HttpStatusCode.OK, dtoToReturn);
        }

        private async Task<ServiceResponse<EditProfileDtoResponse>> EditAdminProfileAsync(EditProfileDtoRequest dto)
        {
            if (await Context.Users.Where(x => x.Email == dto.Email && CurrentlyLoggedUser.Email != dto.Email).AnyAsync())
                return new ServiceResponse<EditProfileDtoResponse>(HttpStatusCode.BadRequest, "Email jest zajęty");

            if (await Context.Users.Where(x => x.UserName == dto.UserName && CurrentlyLoggedUser.UserName != dto.UserName).AnyAsync())
                return new ServiceResponse<EditProfileDtoResponse>(HttpStatusCode.BadRequest, "Nick jest zajęty");

            var anythingChanged = await UpdateProfileAsync(dto);
            int result = await Context.SaveChangesAsync();
            await UserManager.UpdateAsync(CurrentlyLoggedUser);

            var responseDto = Mapper.Map<EditProfileDtoResponse>(CurrentlyLoggedUser);

            if (result > 0 || anythingChanged)
                return new ServiceResponse<EditProfileDtoResponse>(HttpStatusCode.OK, responseDto);
            if (result == 0)
                return new ServiceResponse<EditProfileDtoResponse>(HttpStatusCode.BadRequest, "Nie nastąpiła żadna zmiana");
            return new ServiceResponse<EditProfileDtoResponse>(HttpStatusCode.BadRequest, "Wystąpił błąd podczas zapisu");
        }

        private async Task<ServiceResponse<EditProfileDtoResponse>> EditVetProfileAsync(EditProfileDtoRequest dto)
        {
            var vet = await Context.Vets.SingleOrDefaultAsync(v => v.UserId == CurrentlyLoggedUser.Id);
            if (vet == null)
                return new ServiceResponse<EditProfileDtoResponse>(HttpStatusCode.Unauthorized);

            if (await Context.Users.Where(x => x.Email == dto.Email && CurrentlyLoggedUser.Email != dto.Email).AnyAsync())
                return new ServiceResponse<EditProfileDtoResponse>(HttpStatusCode.BadRequest, "Email jest zajęty");

            if (await Context.Users.Where(x => x.UserName == dto.UserName && CurrentlyLoggedUser.UserName != dto.UserName).AnyAsync())
                return new ServiceResponse<EditProfileDtoResponse>(HttpStatusCode.BadRequest, "Nick jest zajęty");

            var anythingChanged = await UpdateProfileAsync(dto);
            vet.Specialization = dto.Specialization;
            int result = await Context.SaveChangesAsync();
            await UserManager.UpdateAsync(CurrentlyLoggedUser);

            var responseDto = Mapper.Map<EditProfileDtoResponse>(CurrentlyLoggedUser);

            if (result > 0 || anythingChanged)
                return new ServiceResponse<EditProfileDtoResponse>(HttpStatusCode.OK, responseDto);
            if (result == 0)
                return new ServiceResponse<EditProfileDtoResponse>(HttpStatusCode.BadRequest, "Nie nastąpiła żadna zmiana");
            return new ServiceResponse<EditProfileDtoResponse>(HttpStatusCode.BadRequest, "Wystąpił błąd podczas zapisu");
        }

        private async Task<ServiceResponse<EditProfileDtoResponse>> EditOwnerProfileAsync(EditProfileDtoRequest dto)
        {
            var owner = await Context.Owners.SingleOrDefaultAsync(o => o.UserId == CurrentlyLoggedUser.Id);
            if (owner == null)
                return new ServiceResponse<EditProfileDtoResponse>(HttpStatusCode.Unauthorized);

            if (await Context.Users.Where(x => x.Email == dto.Email && CurrentlyLoggedUser.Email != dto.Email).AnyAsync())
                return new ServiceResponse<EditProfileDtoResponse>(HttpStatusCode.BadRequest, "Email jest zajęty");

            if (await Context.Users.Where(x => x.UserName == dto.UserName && CurrentlyLoggedUser.UserName != dto.UserName).AnyAsync())
                return new ServiceResponse<EditProfileDtoResponse>(HttpStatusCode.BadRequest, "Nick jest zajęty");

            var anythingChanged = await UpdateProfileAsync(dto);
            owner.PlaceOfResidence = dto.PlaceOfResidence;
            int result = await Context.SaveChangesAsync();
            await UserManager.UpdateAsync(CurrentlyLoggedUser);

            var responseDto = Mapper.Map<EditProfileDtoResponse>(CurrentlyLoggedUser);

            if (result > 0 || anythingChanged)
                return new ServiceResponse<EditProfileDtoResponse>(HttpStatusCode.OK, responseDto);
            if (result == 0)
                return new ServiceResponse<EditProfileDtoResponse>(HttpStatusCode.BadRequest, "Nie nastąpiła żadna zmiana");
            return new ServiceResponse<EditProfileDtoResponse>(HttpStatusCode.BadRequest, "Wystąpił błąd podczas zapisu");
        }

        private async Task<bool> UpdateProfileAsync(EditProfileDtoRequest dto)
        {
            bool anythingChanged = false;

            if (!CurrentlyLoggedUser.FirstName.Equals(dto.FirstName))
                anythingChanged = true;

            if (!CurrentlyLoggedUser.LastName.Equals(dto.LastName))
                anythingChanged = true;

            if (!CurrentlyLoggedUser.UserName.Equals(dto.UserName))
                anythingChanged = true;

            if (!CurrentlyLoggedUser.Email.Equals(dto.Email))
                anythingChanged = true;

            bool imageAssigned = await ChangeUserImageAsync(CurrentlyLoggedUser, dto.Image);

            return anythingChanged || imageAssigned;
        }

        private async Task<ServiceResponse<RegisterDtoResponse>> ValidateRegisterRequestAsync(RegisterDtoRequest dto)
        {
            if (await Context.Users.Where(x => x.Email == dto.Email).AnyAsync())
                return new ServiceResponse<RegisterDtoResponse>(HttpStatusCode.BadRequest, "Email jest zajęty");

            if (await Context.Users.Where(x => x.UserName == dto.UserName).AnyAsync())
                return new ServiceResponse<RegisterDtoResponse>(HttpStatusCode.BadRequest, "Nick jest zajęty");
            return new ServiceResponse<RegisterDtoResponse>(HttpStatusCode.OK);
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

        public async Task<ServiceResponse> DeleteUserAsync(string userId)
        {
            if (CurrentlyLoggedUser == null)
                return new ServiceResponse(HttpStatusCode.Unauthorized);

            var user = await Context.Users.SingleOrDefaultAsync(x => x.Id == userId);
            if (user == null)
                return new ServiceResponse(HttpStatusCode.NotFound);

            if (CurrentlyLoggedUser.Role == Role.Administrator)
            {
                await clearUserDataAsync(user);
            }
            
            return await Context.SaveChangesAsync() > 0
                ? new ServiceResponse(HttpStatusCode.OK)
                : new ServiceResponse(HttpStatusCode.BadRequest, "Wystąpił błąd podczas usuwania użytkownika");
        }

        private async Task clearUserDataAsync(ApplicationUser user)
        {
            var usrNotes = await Context.Notes.Where(n => n.UserId == user.Id).ToListAsync();
            if (usrNotes.Any())
                Context.Notes.RemoveRange(usrNotes);

            if (user.Role == Role.Vet)
            {
                var vet = await Context.Vets.SingleOrDefaultAsync(x => x.UserId == user.Id);

                var institutionVets = await Context.InstitutionVets.Where(iv => iv.VetId == vet.Id).ToListAsync();
                if (institutionVets.Any())
                { 
                    Context.InstitutionVets.RemoveRange(institutionVets);
                }

            }

            if (user.Role == Role.Owner)
            {
                var pets = await Context.OwnerPets.Where(x => x.Owner.User.Id == user.Id && x.MainOwner)
                                                   .Select(x => x.Pet)
                                                   .ToListAsync();

                if (pets.Any())
                {
                    foreach (Pet pet in pets)
                    {
                        var ownerPets = await Context.OwnerPets.Where(op => op.PetId == pet.Id).ToListAsync();
                        Context.OwnerPets.RemoveRange(ownerPets);

                        var requests = await Context.Requests.Where(r => r.PetId == pet.Id).ToListAsync();
                        if (requests.Any())
                            Context.Requests.RemoveRange(requests);

                        var notes = await Context.Notes.Where(n => n.PetId == pet.Id).ToListAsync();
                        if (notes.Any())
                            Context.Notes.RemoveRange(notes);

                        var vetPets = await Context.VetPets.Where(vp => vp.PetId == pet.Id).ToListAsync();
                        if (vetPets.Any())
                            Context.VetPets.RemoveRange(vetPets);

                        var examinations = await Context.Examinations.Where(e => e.PetId == pet.Id).ToListAsync();

                        if (examinations.Any())
                        {
                            foreach (Examination examination in examinations)
                            {
                                var examinationParameterValue = await Context.ExaminationParameterValues.Where(e => e.ExaminationId == examination.Id).ToListAsync();
                                if (examinationParameterValue.Any())
                                    Context.ExaminationParameterValues.RemoveRange(examinationParameterValue);
                            }
                            Context.Examinations.RemoveRange(examinations);
                        }
                    }
                    Context.Pets.RemoveRange(pets);
                }
                var owner = await Context.Owners.SingleOrDefaultAsync(x => x.UserId == user.Id);
                Context.Owners.Remove(owner);
            }
            Context.Users.Remove(user);
        }

    }
}
