using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Application.Interfaces;
using Application.Dtos.ExaminationTypes;
using Application.Services.Utilities;
using Microsoft.AspNetCore.Authorization;
using Domain.Models;

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

    }
}
