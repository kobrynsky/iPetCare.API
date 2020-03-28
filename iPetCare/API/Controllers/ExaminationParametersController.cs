using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Application.Interfaces;
using Application.Dtos.ExaminationParameters;
using Application.Services.Utilities;
using Microsoft.AspNetCore.Authorization;
using Domain.Models;
using API.Security;

namespace API.Controllers
{
    public class ExaminationParametersController : BaseController
    {
        private readonly IExaminationParameterService _examinationParameterService;
        public ExaminationParametersController(IExaminationParameterService examinationParameterService)
        {
            _examinationParameterService = examinationParameterService;
        }

        [Produces(typeof(ServiceResponse<ExaminationParametersCreateExaminationParameterDtoResponse>))]
        [Authorize(Roles = Role.Administrator)]
        [HttpPost]
        public async Task<IActionResult> CreateExaminationParameter(ExaminationParametersCreateExaminationParameterDtoRequest dto)
        {
            var response = await _examinationParameterService.CreateExaminationParameterAsync(dto);
            return SendResponse(response);
        }
    }
}
