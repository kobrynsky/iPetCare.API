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

        [Produces(typeof(ServiceResponse<InstitutionsGetInstitutionsDtoResponse>))]
        [HttpGet]
        public async Task<IActionResult> GetInstitutions()
        {
            var response = await _institutionService.GetInstitutionsAsync();
            return SendResponse(response);
        }

        [Produces(typeof(ServiceResponse<InstitutionsGetInstitutionDtoResponse>))]
        [HttpGet("{institutionId}")]
        public async Task<IActionResult> GetInstitution(Guid institutionId)
        {
            var response = await _institutionService.GetInstitutionAsync(institutionId);
            return SendResponse(response);
        }

        [Produces(typeof(ServiceResponse<InstitutionsCreateInstitutionDtoResponse>))]
        [Authorize(Roles = Role.Administrator)]
        [HttpPost]
        public async Task<IActionResult> CreateInstitution([FromBody] InstitutionsCreateInstitutionDtoRequest dto)
        {
            var response = await _institutionService.CreateInstitutionAsync(dto);
            return SendResponse(response);
        }

        [Produces(typeof(ServiceResponse<InstitutionsUpdateInstitutionDtoResponse>))]
        [Authorize(Roles = Role.Administrator)]
        [HttpPut("{institutionId}")]
        public async Task<IActionResult> UpdateInstitution(Guid institutionId, [FromBody] InstitutionsUpdateInstitutionDtoRequest dto)
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
    }
}