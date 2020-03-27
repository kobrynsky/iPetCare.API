using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Application.Interfaces;
using Application.Dtos.ExaminationTypes;
using Application.Services.Utilities;
using Microsoft.AspNetCore.Authorization;
using Domain.Models;
using API.Security;

namespace API.Controllers
{
    [ApiController]
    public class ExaminationTypesController : BaseController
    {
        private readonly IExaminationTypes _examinationTypesService;
        public ExaminationTypesController(IExaminationTypes examinationTypesService)
        {
            _examinationTypesService = examinationTypesService;
        }

        [Produces(typeof(ServiceResponse<ExaminationTypesCreateDtoResponse>))]
        [Authorize(Roles = Role.Administrator)]
        [HttpPost]
        public async Task<IActionResult> CreateExaminationTypes(ExaminationTypesCreateDtoRequest dto)
        {
            var response = await _examinationTypesService.CreateExaminationTypesAsync(dto);
            return SendResponse(response);
        }

        [Produces(typeof(ServiceResponse<ExaminationTypesGetAllDtoResponse>))]
        [AuthorizeRoles(Role.Administrator, Role.Vet, Role.Owner)]
        [HttpGet]
        public async Task<IActionResult> GetAllExaminationTypes()
        {
            var response = await _examinationTypesService.GetAllExaminationTypesAsync();
            return SendResponse(response);
        }

        [Produces(typeof(ServiceResponse<ExaminationTypesGetAllDtoResponse>))]
        [AuthorizeRoles(Role.Administrator, Role.Vet, Role.Owner)]
        [HttpGet("{examinationTypeId}")]
        public async Task<IActionResult> GetExaminationType(int examinationTypeId)
        {
            var response = await _examinationTypesService.GetExaminationTypeAsync(examinationTypeId);
            return SendResponse(response);
        }

        [Produces(typeof(ServiceResponse<ExaminationTypesCreateDtoResponse>))]
        [Authorize(Roles = Role.Administrator)]
        [HttpPut("{examinationTypeId}")]
        public async Task<IActionResult> UpdateExaminationType(int examinationTypeId, ExaminationTypeUpdateDtoRequest dto)
        {
            var response = await _examinationTypesService.UpdateExaminationTypeAsync(examinationTypeId, dto);
            return SendResponse(response);
        }

        [Produces(typeof(ServiceResponse))]
        [Authorize(Roles = Role.Administrator)]
        [HttpDelete("{examinationTypeId}")]
        public async Task<IActionResult> DeleteExaminationType(int examinationTypeId)
        {
            var response = await _examinationTypesService.DeleteExaminationTypeAsync(examinationTypeId);
            return SendResponse(response);
        }
    }
}
