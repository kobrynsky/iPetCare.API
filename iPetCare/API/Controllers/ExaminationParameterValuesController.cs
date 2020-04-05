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

        [Produces(typeof(ServiceResponse<ExaminationParameterValuesCreateExaminationParameterValueDtoResponse>))]
        [Authorize(Roles = Role.Administrator)]
        [HttpPost]
        public async Task<IActionResult> CreateExaminationParameterValue(CreateExaminationParameterValueDtoRequest dto)
        {
            var response = await _examinationParameterValueService.CreateExaminationParameterValueAsync(dto);
            return SendResponse(response);
        }

        [Produces(typeof(ServiceResponse<ExaminationParameterValuesGetAllExaminationParametersValuesDtoResponse>))]
        [AuthorizeRoles(Role.Administrator, Role.Vet, Role.Owner)]
        [HttpGet]
        public async Task<IActionResult> GetAllExaminationParametersValues()
        {
            var response = await _examinationParameterValueService.GetAllExaminationParametersValuesAsync();
            return SendResponse(response);
        }

        [Produces(typeof(ServiceResponse<ExaminationParameterValuesGetExaminationParameterValueDtoResponse>))]
        [AuthorizeRoles(Role.Administrator, Role.Vet, Role.Owner)]
        [HttpGet("{examinationParameterId}")]
        public async Task<IActionResult> GetExaminationParameterValue(int examinationParameterId)
        {
            var response = await _examinationParameterValueService.GetExaminationParameterValueAsync(examinationParameterId);
            return SendResponse(response);
        }

        [Produces(typeof(ServiceResponse<ExaminationParameterValuesUpdateExaminationParameterValueDtoResponse>))]
        [Authorize(Roles = Role.Administrator)]
        [HttpPut("{examinationParameterId}")]
        public async Task<IActionResult> UpdateExaminationParameterValue(int examinationParameterId, ExaminationParameterValuesUpdateExaminationParameterValueDtoRequest dto)
        {
            var response = await _examinationParameterValueService.UpdateExaminationParameterValueAsync(examinationParameterId, dto);
            return SendResponse(response);
        }

        [Produces(typeof(ServiceResponse))]
        [Authorize(Roles = Role.Administrator)]
        [HttpDelete("{examinationParameterId}")]
        public async Task<IActionResult> DeleteExaminationParameterValue(int examinationParameterId)
        {
            var response = await _examinationParameterValueService.DeleteExaminationParameterValueAsync(examinationParameterId);
            return SendResponse(response);
        }
    }
}
