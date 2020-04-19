using System.Threading.Tasks;
using API.Security;
using Application.Dtos.Species;
using Application.Interfaces;
using Application.Services.Utilities;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class SpeciesController : BaseController
    {
        private readonly ISpeciesService _speciesService;

        public SpeciesController(ISpeciesService speciesService)
        {
            _speciesService = speciesService;
        }

        [Produces(typeof(ServiceResponse<CreateSpeciesDtoResponse>))]
        [Authorize(Roles = Role.Administrator)]
        [HttpPost]
        public async Task<IActionResult> CreateSpecies(CreateSpeciesDtoRequest dto)
        {
            var response = await _speciesService.CreateSpeciesAsync(dto);
            return SendResponse(response);
        }

        [Produces(typeof(ServiceResponse<GetAllSpeciesDtoResponse>))]
        [AuthorizeRoles(Role.Administrator, Role.Vet, Role.Owner)]
        [HttpGet]
        public async Task<IActionResult> GetAllSpecies()
        {
            var response = await _speciesService.GetAllSpeciesAsync();
            return SendResponse(response);
        }

        [Produces(typeof(ServiceResponse<GetSpeciesDtoResponse>))]
        [AuthorizeRoles(Role.Administrator, Role.Vet, Role.Owner)]
        [HttpGet("{speciesId}")]
        public async Task<IActionResult> GetSpecies(int speciesId)
        {
            var response = await _speciesService.GetSpeciesAsync(speciesId);
            return SendResponse(response);
        }

        [Produces(typeof(ServiceResponse<UpdateSpeciesDtoResponse>))]
        [Authorize(Roles = Role.Administrator)]
        [HttpPut("{speciesId}")]
        public async Task<IActionResult> UpdateSpecies(int speciesId, UpdateSpeciesDtoRequest dto)
        {
            var response = await _speciesService.UpdateSpeciesAsync(speciesId, dto);
            return SendResponse(response);
        }

        [Produces(typeof(ServiceResponse<DeleteSpeciesDtoResponse>))]
        [Authorize(Roles = Role.Administrator)]
        [HttpDelete("{speciesId}")]
        public async Task<IActionResult> DeleteSpecies(int speciesId)
        {
            var response = await _speciesService.DeleteSpeciesAsync(speciesId);
            return SendResponse(response);
        }
    }
}