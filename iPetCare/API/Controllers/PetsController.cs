using System;
using System.Threading.Tasks;
using API.Security;
using Application.Dtos.Pet;
using Application.Interfaces;
using Application.Services.Utilities;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class PetsController : BaseController
    {
        private readonly IPetService _petService;

        public PetsController(IPetService petService)
        {
            _petService = petService;
        }

        [Produces(typeof(ServiceResponse<GetPetsDtoResponse>))]
        [AuthorizeRoles(Roles = Role.Administrator)]
        [HttpGet]
        public async Task<IActionResult> GetPets()
        {
            var response = await _petService.GetPetsAsync();
            return SendResponse(response);
        }

        [Produces(typeof(ServiceResponse<GetMyPetsDtoResponse>))]
        [Authorize(Roles = Role.Owner)]
        [HttpGet("my")]
        public async Task<IActionResult> GetMyPets()
        {
            var response = await _petService.GetMyPetsAsync();
            return SendResponse(response);
        }

        [Produces(typeof(ServiceResponse<GetUserPetsDtoResponse>))]
        [AuthorizeRoles(Role.Owner, Role.Vet)]
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserPets(string userId)
        {
            var response = await _petService.GetUserPetsAsync(userId);
            return SendResponse(response);
        }

        [Produces(typeof(ServiceResponse<GetSharedPetsDtoResponse>))]
        [AuthorizeRoles(Role.Owner, Role.Vet)]
        [HttpGet("shared")]
        public async Task<IActionResult> GetSharedPets()
        {
            var response = await _petService.GetSharedPetsAsync();
            return SendResponse(response);
        }

        [Produces(typeof(ServiceResponse<GetPetDtoResponse>))]
        [HttpGet("{petId}")]
        public async Task<IActionResult> GetPet(Guid petId)
        {
            var response = await _petService.GetPetAsync(petId);
            return SendResponse(response);
        }

        [Produces(typeof(ServiceResponse<CreatePetDtoResponse>))]
        [HttpPost]
        [Authorize(Roles = Role.Owner)]
        public async Task<IActionResult> CreatePet([FromForm] CreatePetDtoRequest dto)
        {
            var response = await _petService.CreatePetAsync(dto);
            return SendResponse(response);
        }

        [Produces(typeof(ServiceResponse<UpdatePetDtoResponse>))]
        [HttpPut("{petId}")]
        public async Task<IActionResult> UpdatePet(Guid petId, [FromForm] UpdatePetDtoRequest dto)
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
    }
}
