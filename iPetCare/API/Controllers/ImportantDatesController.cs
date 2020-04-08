using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Application.Interfaces;
using Application.Dtos.ImportantDates;
using Application.Services.Utilities;
using Domain.Models;
using API.Security;

namespace API.Controllers
{
    public class ImportantDatesController : BaseController
    {

        private readonly IImportantDatesService _importantDatesService;
        public ImportantDatesController(IImportantDatesService importantDatesService)
        {
            _importantDatesService = importantDatesService;
        }

        [Produces(typeof(ServiceResponse<CreateImportantDateDtoResponse>))]
        [AuthorizeRoles(Role.Administrator, Role.Vet, Role.Owner)]
        [HttpPost]
        public async Task<IActionResult> CreateImportantDate(CreateImportantDateDtoRequest dto)
        {
            var response = await _importantDatesService.CreateImportantDateAsync(dto);
            return SendResponse(response);
        }

        [Produces(typeof(ServiceResponse<GetAllImportantDatesDtoResponse>))]
        [AuthorizeRoles(Role.Administrator)]
        [HttpGet]
        public async Task<IActionResult> GetAllImportantDates()
        {
            var response = await _importantDatesService.GetAllImportantDatesAsync();
            return SendResponse(response);
        }

        [Produces(typeof(ServiceResponse<GetImportantDateDtoResponse>))]
        [AuthorizeRoles(Role.Administrator, Role.Vet, Role.Owner)]
        [HttpGet("{importantDateId}")]
        public async Task<IActionResult> GetImportantDate(Guid importantDateId)
        {
            var response = await _importantDatesService.GetImportantDateAsync(importantDateId);
            return SendResponse(response);
        }

        [Produces(typeof(ServiceResponse<CreateImportantDateDtoResponse>))]
        [AuthorizeRoles(Role.Administrator, Role.Vet, Role.Owner)]
        [HttpPut("{importantDateId}")]
        public async Task<IActionResult> UpdateImportantDate(Guid importantDateId, UpdateImportantDateDtoRequest dto)
        {
            var response = await _importantDatesService.UpdateImportantDateAsync(importantDateId, dto);
            return SendResponse(response);
        }

        [Produces(typeof(ServiceResponse))]
        [AuthorizeRoles(Role.Administrator, Role.Vet, Role.Owner)]
        [HttpDelete("{importantDateId}")]
        public async Task<IActionResult> DeleteImportantDate(Guid importantDateId)
        {
            var response = await _importantDatesService.DeleteImportantDateAsync(importantDateId);
            return SendResponse(response);
        }
    }
}
