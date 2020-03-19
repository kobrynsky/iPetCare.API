using System.Threading.Tasks;
using Application.Dtos.Races;
using Application.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

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
        [HttpPost]
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
    }
}