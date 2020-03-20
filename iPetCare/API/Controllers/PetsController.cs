using System;
using System.Net;
using System.Threading.Tasks;
using Application.Dtos.Pet;
using Application.Interfaces;
using Application.Services.Utilities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PetsController : ControllerBase
    {
        private readonly IPetService _petService;

        public PetsController(IPetService petService)
        {
            _petService = petService;
        }

        [Produces(typeof(ServiceResponse<PetsGetPetsDtoResponse>))]
        [HttpGet]
        public async Task<IActionResult> GetPets()
        {
            var response = await _petService.GetPetsAsync();
            return SendResponse(response);
        }

        [Produces(typeof(ServiceResponse<PetsGetPetDtoResponse>))]
        [HttpGet("{petId}")]
        public async Task<IActionResult> GetPet(Guid petId)
        {
            var response = await _petService.GetPetAsync(petId);
            return SendResponse(response);
        }

        [Produces(typeof(ServiceResponse<PetsCreatePetDtoResponse>))]
        [HttpPost]
        public async Task<IActionResult> CreatePet([FromBody] PetsCreatePetDtoRequest dto)
        {
            var response = await _petService.CreatePetAsync(dto);
            return SendResponse(response);
        }

        [Produces(typeof(ServiceResponse<PetsUpdatePetDtoResponse>))]
        [HttpPut("{petId}")]
        public async Task<IActionResult> UpdatePet(Guid petId, [FromBody] PetsUpdatePetDtoRequest dto)
        {
            var response = await _petService.UpdatePetAsync(petId, dto);
            return SendResponse(response);
        }

        [Produces(typeof(ServiceResponse))]
        [HttpDelete("{petId}")]
        public async Task<IActionResult> DeletePet(Guid petId)
        {
            var response = await _petService.DeletePetAsync(petId);
            return SendResponse(response);
        }

        private IActionResult SendResponse(ServiceResponse response)
        {
            switch (response.ResponseType)
            {
                case HttpStatusCode.OK:
                    return Ok();
                case HttpStatusCode.Unauthorized:
                    return Unauthorized(response.Message);
                case HttpStatusCode.Forbidden:
                    return Forbid(response.Message);
                default:
                    return BadRequest(response.Message);
            }
        }

        private IActionResult SendResponse<T>(ServiceResponse<T> response)
        {
            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    return Ok(response.ResponseContent);
                case HttpStatusCode.Unauthorized:
                    return StatusCode(401, response.Message);
                case HttpStatusCode.Forbidden:
                    return Forbid();
                default:
                    return BadRequest(response.Message);
            }
        }
    }
}
