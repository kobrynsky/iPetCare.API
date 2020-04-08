using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Application.Interfaces;
using Application.Dtos.ExaminationParameterValues;
using Application.Services.Utilities;
using Microsoft.AspNetCore.Authorization;
using Domain.Models;
using API.Security;

namespace API.Controllers
{
    public class ExaminationParameterValuesController : BaseController
    {
        private readonly IExaminationParameterValueService _examinationParameterValueService;
        public ExaminationParameterValuesController(IExaminationParameterValueService examinationParameterValueService)
        {
            _examinationParameterValueService = examinationParameterValueService;
        }

        [Produces(typeof(ServiceResponse<CreateExaminationParameterValueDtoResponse>))]
        [AuthorizeRoles(Role.Administrator, Role.Vet, Role.Owner)]
        [HttpPost]
        public async Task<IActionResult> CreateExaminationParameterValue(CreateExaminationParameterValueDtoRequest dto)
        {
            var response = await _examinationParameterValueService.CreateExaminationParameterValueAsync(dto);
            return SendResponse(response);
        }

        [Produces(typeof(ServiceResponse<GetAllExaminationParametersValuesDtoResponse>))]
        [Authorize(Roles = Role.Administrator)]
        [HttpGet]
        public async Task<IActionResult> GetAllExaminationParametersValues()
        {
            var response = await _examinationParameterValueService.GetAllExaminationParametersValuesAsync();
            return SendResponse(response);
        }

        [Produces(typeof(ServiceResponse<GetExaminationParameterValueDtoResponse>))]
        [AuthorizeRoles(Role.Administrator, Role.Vet, Role.Owner)]
        [HttpGet("{examinationParameterValueId}")]
        public async Task<IActionResult> GetExaminationParameterValue(Guid examinationParameterValueId)
        {
            var response = await _examinationParameterValueService.GetExaminationParameterValueAsync(examinationParameterValueId);
            return SendResponse(response);
        }

        [Produces(typeof(ServiceResponse<UpdateExaminationParameterValueDtoResponse>))]
        [AuthorizeRoles(Role.Administrator, Role.Vet, Role.Owner)]
        [HttpPut("{examinationParameterId}")]
        public async Task<IActionResult> UpdateExaminationParameterValue(Guid examinationParameterId, UpdateExaminationParameterValueDtoRequest dto)
        {
            var response = await _examinationParameterValueService.UpdateExaminationParameterValueAsync(examinationParameterId, dto);
            return SendResponse(response);
        }

        [Produces(typeof(ServiceResponse))]
        [AuthorizeRoles(Role.Administrator, Role.Vet, Role.Owner)]
        [HttpDelete("{examinationParameterId}")]
        public async Task<IActionResult> DeleteExaminationParameterValue(Guid examinationParameterId)
        {
            var response = await _examinationParameterValueService.DeleteExaminationParameterValueAsync(examinationParameterId);
            return SendResponse(response);
        }
    }
}
