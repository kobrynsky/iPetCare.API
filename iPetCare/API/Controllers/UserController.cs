using System.Net;
using System.Threading.Tasks;
using API.Security;
using Application.Dtos.Users;
using Application.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<LoginDtoResponse>> Login(LoginDtoRequest dto)
        {
            var response = await _userService.LoginAsync(dto);

            if (response.StatusCode == HttpStatusCode.OK)
                return Ok(response.ResponseContent);
            if (response.StatusCode == HttpStatusCode.Unauthorized)
                return Unauthorized(response.Message);
            if (response.StatusCode == HttpStatusCode.Forbidden)
                return Forbid(response.Message);
            return BadRequest(response.Message);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<RegisterDtoResponse>> Register(RegisterDtoRequest dto)
        {
            var response =  await _userService.RegisterAsync(dto);

            if (response.StatusCode == HttpStatusCode.OK)
                return Ok(response.ResponseContent);
            if (response.StatusCode == HttpStatusCode.Unauthorized)
                return Unauthorized(response.Message);
            if (response.StatusCode == HttpStatusCode.Forbidden)
                return Forbid(response.Message);
            return BadRequest(response.Message);
        }

        [Authorize(Roles = Role.Administrator)]
        [HttpGet("")]
        public async Task<ActionResult<GetAllUsersDtoResponse>> GetUsers()
        {
            var response = await _userService.GetAllAsync();

            if (response.StatusCode == HttpStatusCode.OK)
                return Ok(response.ResponseContent);
            if (response.StatusCode == HttpStatusCode.Unauthorized)
                return Unauthorized(response.Message);
            if (response.StatusCode == HttpStatusCode.Forbidden)
                return Forbid(response.Message);
            return BadRequest(response.Message);
        }

        [AuthorizeRoles(Role.Administrator, Role.Vet, Role.Owner)]
        [HttpPut]
        public async Task<ActionResult<EditProfileDtoResponse>> EditProfile(EditProfileDtoRequest dto)
        {
            var response = await _userService.EditProfileAsync(dto);

            if (response.StatusCode == HttpStatusCode.OK)
                return Ok(response.ResponseContent);
            if (response.StatusCode == HttpStatusCode.Unauthorized)
                return Unauthorized(response.Message);
            if (response.StatusCode == HttpStatusCode.Forbidden)
                return Forbid(response.Message);
            return BadRequest(response.Message);
        }
    }
}