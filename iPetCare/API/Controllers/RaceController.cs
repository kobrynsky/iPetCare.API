using System.Threading.Tasks;
using Application.Dtos.Races;
using Application.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using API.Security;

namespace API.Controllers
{
    [Route("api/races")]
    [ApiController]
    public class RaceController : Controller
    {
        private readonly IRaceService _raceService;

        public RaceController(IRaceService raceService)
        {
            _raceService = raceService;
        }

        [Authorize(Roles = Role.Administrator)]
        [HttpPost("")]
        public async Task<ActionResult<CreateDtoResponse>> Create(CreateDtoRequest dto)
        {
            var response = await _raceService.CreateAsync(dto);

            if (response.StatusCode == HttpStatusCode.OK)
                return Ok(response.ResponseContent);
            if (response.StatusCode == HttpStatusCode.Unauthorized)
                return Unauthorized(response.Message);
            if (response.StatusCode == HttpStatusCode.Forbidden)
                return Forbid(response.Message);
            return BadRequest(response.Message);
        }

        [AuthorizeRoles(Role.Administrator, Role.Vet, Role.Owner)]
        [HttpGet("")]
        public async Task<ActionResult<GetAllDtoResponse>> GetRaces()
        {
            var response = await _raceService.GetAllAsync();

            if (response.StatusCode == HttpStatusCode.OK)
                return Ok(response.ResponseContent);
            if (response.StatusCode == HttpStatusCode.Unauthorized)
                return Unauthorized(response.Message);
            if (response.StatusCode == HttpStatusCode.Forbidden)
                return Forbid(response.Message);
            return BadRequest(response.Message);
        }

        [AuthorizeRoles(Role.Administrator, Role.Vet, Role.Owner)]
        [HttpGet("{raceId}")]
        public async Task<ActionResult<GetDtoResponse>> GetRace(int raceId)
        {
            var response = await _raceService.GetAsync(raceId);

            if (response.StatusCode == HttpStatusCode.OK)
                return Ok(response.ResponseContent);
            if (response.StatusCode == HttpStatusCode.Unauthorized)
                return Unauthorized(response.Message);
            if (response.StatusCode == HttpStatusCode.Forbidden)
                return Forbid(response.Message);
            return BadRequest(response.Message);
        }

        [Authorize(Roles = Role.Administrator)]
        [HttpPut("{raceId}")]
        public async Task<ActionResult<PutDtoResponse>> PutRace(int raceId, PutDtoRequest dto)
        {
            var response = await _raceService.PutAsync(raceId, dto);

            if (response.StatusCode == HttpStatusCode.OK)
                return Ok(response.ResponseContent);
            if (response.StatusCode == HttpStatusCode.Unauthorized)
                return Unauthorized(response.Message);
            if (response.StatusCode == HttpStatusCode.Forbidden)
                return Forbid(response.Message);
            return BadRequest(response.Message);
        }

        [Authorize(Roles = Role.Administrator)]
        [HttpDelete("{raceId}")]
        public async Task<ActionResult<DeleteDtoResponse>> DeleteRace(int raceId)
        {
            var response = await _raceService.DeleteAsync(raceId);

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