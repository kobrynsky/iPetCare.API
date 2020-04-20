using System.Threading.Tasks;
using Application.Dtos.Races;
using Application.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using API.Security;
using Application.Services.Utilities;

namespace API.Controllers
{
    public class RacesController : BaseController
    {
        private readonly IRaceService _raceService;

        public RacesController(IRaceService raceService)
        {
            _raceService = raceService;
        }

        [Produces(typeof(ServiceResponse<CreateRaceDtoResponse>))]
        [Authorize(Roles = Role.Administrator)]
        [HttpPost]
        public async Task<IActionResult> Create(CreateRaceDtoRequest dto)
        {
            var response = await _raceService.CreateRaceAsync(dto);
            return SendResponse(response);
        }

        [Produces(typeof(ServiceResponse<GetAllRacesDtoResponse>))]
        [AuthorizeRoles(Role.Administrator, Role.Vet, Role.Owner)]
        [HttpGet]
        public async Task<IActionResult> GetRaces()
        {
            var response = await _raceService.GetAllRacesAsync();
            return SendResponse(response);
        }

        [Produces(typeof(ServiceResponse<GetRaceDtoResponse>))]
        [AuthorizeRoles(Role.Administrator, Role.Vet, Role.Owner)]
        [HttpGet("{raceId}")]
        public async Task<IActionResult> GetRace(int raceId)
        {
            var response = await _raceService.GetRaceAsync(raceId);
            return SendResponse(response);
        }

        [Produces(typeof(ServiceResponse<UpdateRaceDtoResponse>))]
        [Authorize(Roles = Role.Administrator)]
        [HttpPut("{raceId}")]
        public async Task<IActionResult> UpdateRace(int raceId, UpdateRaceDtoRequest dto)
        {
            var response = await _raceService.UpdateRaceAsync(raceId, dto);
            return SendResponse(response);
        }

        [Produces(typeof(ServiceResponse<DeleteRaceDtoResponse>))]
        [Authorize(Roles = Role.Administrator)]
        [HttpDelete("{raceId}")]
        public async Task<IActionResult> DeleteRace(int raceId)
        {
            var response = await _raceService.DeleteRaceAsync(raceId);
            return SendResponse(response);
        }
    }
}