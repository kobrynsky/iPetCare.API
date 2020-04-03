using System;
using System.Threading.Tasks;
using API.Security;
using Application.Dtos.Institutions;
using Application.Interfaces;
using Application.Services.Utilities;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class InstitutionsController : BaseController
    {
        private readonly IInstitutionService _institutionService;

        public InstitutionsController(IInstitutionService institutionService)
        {
            _institutionService = institutionService;
        }

        [Produces(typeof(ServiceResponse<GetInstitutionsDtoResponse>))]
        [HttpGet]
        public async Task<IActionResult> GetInstitutions()
        {
            var response = await _institutionService.GetInstitutionsAsync();
            return SendResponse(response);
        }

        [Produces(typeof(ServiceResponse<GetInstitutionDtoResponse>))]
        [HttpGet("{institutionId}")]
        public async Task<IActionResult> GetInstitution(Guid institutionId)
        {
            var response = await _institutionService.GetInstitutionAsync(institutionId);
            return SendResponse(response);
        }

        [Produces(typeof(ServiceResponse<CreateInstitutionDtoResponse>))]
        [Authorize(Roles = Role.Administrator)]
        [HttpPost]
        public async Task<IActionResult> CreateInstitution([FromBody] CreateInstitutionDtoRequest dto)
        {
            var response = await _institutionService.CreateInstitutionAsync(dto);
            return SendResponse(response);
        }

        [Produces(typeof(ServiceResponse<UpdateInstitutionDtoResponse>))]
        [Authorize(Roles = Role.Administrator)]
        [HttpPut("{institutionId}")]
        public async Task<IActionResult> UpdateInstitution(Guid institutionId, [FromBody] UpdateInstitutionDtoRequest dto)
        {
            var response = await _institutionService.UpdateInstitutionAsync(institutionId, dto);
            return SendResponse(response);
        }

        [Produces(typeof(ServiceResponse))]
        [Authorize(Roles = Role.Administrator)]
        [HttpDelete("{institutionId}")]
        public async Task<IActionResult> DeleteInstitution(Guid institutionId)
        {
            var response = await _institutionService.DeleteInstitutionAsync(institutionId);
            return SendResponse(response);
        }

        [Produces(typeof(ServiceResponse))]
        [Authorize(Roles = Role.Vet)]
        [HttpPost("signup/{institutionId}")]
        public async Task<IActionResult> SignUp(Guid institutionId)
        {
            var response = await _institutionService.SignUpAsync(institutionId);
            return SendResponse(response);
        }

        [Produces(typeof(ServiceResponse))]
        [Authorize(Roles = Role.Vet)]
        [HttpDelete("signout/{institutionId}")]
        public async Task<IActionResult> SignOut(Guid institutionId)
        {
            var response = await _institutionService.SignOutAsync(institutionId);
            return SendResponse(response);
        }

    }
}