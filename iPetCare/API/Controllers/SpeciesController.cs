using System.Net;
using System.Threading.Tasks;
using API.Security;
using Application.Dtos.Species;
using Application.Dtos.Users;
using Application.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/species")]
    [ApiController]
    public class SpeciesController : Controller
    {
        private readonly ISpeciesService _speciesService;

        public SpeciesController(ISpeciesService speciesService)
        {
            _speciesService = speciesService;
        }

        [Authorize(Roles = Role.Administrator)]
        [HttpPost]
        public async Task<ActionResult<SpeciesCreateDtoResponse>> Create(SpeciesCreateDtoRequest dto)
        {
            var response = await _speciesService.CreateAsync(dto);

            if (response.StatusCode == HttpStatusCode.OK)
                return Ok(response.ResponseContent);
            if (response.StatusCode == HttpStatusCode.Unauthorized)
                return Unauthorized(response.Message);
            if (response.StatusCode == HttpStatusCode.Forbidden)
                return Forbid(response.Message);
            return BadRequest(response.Message);
        }

        [AuthorizeRoles(Role.Administrator, Role.Vet, Role.Owner)]
        [HttpGet]
        public async Task<ActionResult<SpeciesGetAllDtoResponse>> GetGetAllSpecies()
        {
            var response = await _speciesService.GetAllAsync();

            if (response.StatusCode == HttpStatusCode.OK)
                return Ok(response.ResponseContent);
            if (response.StatusCode == HttpStatusCode.Unauthorized)
                return Unauthorized(response.Message);
            if (response.StatusCode == HttpStatusCode.Forbidden)
                return Forbid(response.Message);
            return BadRequest(response.Message);
        }

        [AuthorizeRoles(Role.Administrator, Role.Vet, Role.Owner)]
        [HttpGet("{speciesId}")]
        public async Task<ActionResult<SpeciesGetDtoResponse>> GetSpecies(int speciesId)
        {
            var response = await _speciesService.GetAsync(speciesId);

            if (response.StatusCode == HttpStatusCode.OK)
                return Ok(response.ResponseContent);
            if (response.StatusCode == HttpStatusCode.Unauthorized)
                return Unauthorized(response.Message);
            if (response.StatusCode == HttpStatusCode.Forbidden)
                return Forbid(response.Message);
            return BadRequest(response.Message);
        }

        [Authorize(Roles = Role.Administrator)]
        [HttpPut("{speciesId}")]
        public async Task<ActionResult<SpeciesUpdateDtoResponse>> UpdateSpecies(int speciesId, SpeciesUpdateDtoRequest dto)
        {
            var response = await _speciesService.UpdateAsync(speciesId, dto);

            if (response.StatusCode == HttpStatusCode.OK)
                return Ok(response.ResponseContent);
            if (response.StatusCode == HttpStatusCode.Unauthorized)
                return Unauthorized(response.Message);
            if (response.StatusCode == HttpStatusCode.Forbidden)
                return Forbid(response.Message);
            return BadRequest(response.Message);
        }

        [Authorize(Roles = Role.Administrator)]
        [HttpDelete("{speciesId}")]
        public async Task<ActionResult<SpeciesDeleteDtoResponse>> DeleteSpecies(int speciesId)
        {
            var response = await _speciesService.DeleteAsync(speciesId);

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