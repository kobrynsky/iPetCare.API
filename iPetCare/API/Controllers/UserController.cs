using System.Threading.Tasks;
using API.Security;
using Application.Dtos.Owners;
using Application.Dtos.Users;
using Application.Dtos.Vets;
using Application.Interfaces;
using Application.Services.Utilities;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : BaseController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [Produces(typeof(ServiceResponse<LoginDtoResponse>))]
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDtoRequest dto)
        {
            var response = await _userService.LoginAsync(dto);
            return SendResponse(response);
        }

        [Produces(typeof(ServiceResponse<RegisterDtoResponse>))]
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDtoRequest dto)
        {
            var response =  await _userService.RegisterAsync(dto);
            return SendResponse(response);
        }

        [Produces(typeof(ServiceResponse<GetAllUsersDtoResponse>))]
        [Authorize(Roles = Role.Administrator)]
        [HttpGet("")]
        public async Task<IActionResult> GetUsers()
        {
            var response = await _userService.GetAllAsync();
            return SendResponse(response);
        }

        [Produces(typeof(ServiceResponse<EditProfileDtoResponse>))]
        [AuthorizeRoles(Role.Administrator, Role.Vet, Role.Owner)]
        [HttpPut]
        public async Task<IActionResult> EditProfile([FromForm] EditProfileDtoRequest dto)
        {
            var response = await _userService.EditProfileAsync(dto);
            return SendResponse(response);
        }

        [Produces(typeof(ServiceResponse<GetVetsDtoResponse>))]
        [Authorize]
        [HttpPost("vets")]
        public async Task<IActionResult> GetVets([FromBody] GetVetsDtoRequest dto)
        {
            var response = await _userService.GetVetsAsync(dto);
            return SendResponse(response);
        }

        [Produces(typeof(ServiceResponse<GetOwnersDtoResponse>))]
        [Authorize]
        [HttpPost("owners")]
        public async Task<IActionResult> GetOwners([FromBody] GetOwnersDtoRequest dto)
        {
            var response = await _userService.GetOwnersAsync(dto);
            return SendResponse(response);
        }
    }
}