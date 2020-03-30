using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Application.Interfaces;
using Application.Dtos.Examinations;
using Application.Services.Utilities;
using Microsoft.AspNetCore.Authorization;
using Domain.Models;
using API.Security;

namespace API.Controllers
{
    public class ExaminationsController : BaseController
    {

        private readonly IExaminationService _examinationsService;
        public ExaminationsController(IExaminationService examinationsService)
        {
            _examinationsService = examinationsService;
        }

        [Produces(typeof(ServiceResponse<ExaminationsCreateExaminationDtoResponse>))]
        [AuthorizeRoles(Role.Administrator, Role.Vet, Role.Owner)]
        [HttpPost]
        public async Task<IActionResult> CreateExamination(ExaminationsCreateExaminationDtoRequest dto)
        {
            var response = await _examinationsService.CreateExaminationAsync(dto);
            return SendResponse(response);
        }

        [Produces(typeof(ServiceResponse<ExaminationsGetAllExaminationsDtoResponse>))]
        [AuthorizeRoles(Role.Administrator)]
        [HttpGet]
        public async Task<IActionResult> GetAllExaminations()
        {
            var response = await _examinationsService.GetAllExaminationsAsync();
            return SendResponse(response);
        }
        [Produces(typeof(ServiceResponse<ExaminationsGetAllExaminationsDtoResponse>))]
        [AuthorizeRoles(Role.Administrator, Role.Vet, Role.Owner)]
        [HttpGet("{petId}")]
        public async Task<IActionResult> GetPetExaminations(string petId)
        {
            var response = await _examinationsService.GetPetExaminationsAsync(petId);
            return SendResponse(response);
        }
        [Produces(typeof(ServiceResponse<ExaminationsGetAllExaminationsDtoResponse>))]
        [AuthorizeRoles(Role.Administrator, Role.Vet, Role.Owner)]
        [HttpGet("{petId}/{examinationId}")]
        public async Task<IActionResult> GetExamination(string petId, string examinationId)
        {
            var response = await _examinationsService.GetExaminationAsync(petId, examinationId);
            return SendResponse(response);
        }

        [Produces(typeof(ServiceResponse<ExaminationsCreateExaminationDtoResponse>))]
        [AuthorizeRoles(Role.Administrator, Role.Vet, Role.Owner)]
        [HttpPut("{petId}/{examinationId}")]
        public async Task<IActionResult> UpdateExamination(string petId, string examinationId, ExaminationsUpdateExaminationDtoRequest dto)
        {
            var response = await _examinationsService.UpdateExaminationAsync(petId, examinationId, dto);
            return SendResponse(response);
        }

        [Produces(typeof(ServiceResponse))]
        [AuthorizeRoles(Role.Administrator, Role.Vet, Role.Owner)]
        [HttpDelete("{petId}/{examinationId}")]
        public async Task<IActionResult> DeleteExamination(string petId, string examinationId)
        {
            var response = await _examinationsService.DeleteExaminationAsync(petId, examinationId);
            return SendResponse(response);
        }
    }
}
