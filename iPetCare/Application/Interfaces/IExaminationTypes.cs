using System;
using System.Threading.Tasks;
using Application.Dtos.ExaminationTypes;
using Application.Services.Utilities;

namespace Application.Interfaces
{
    public interface IExaminationTypes
    {
        Task<ServiceResponse<ExaminationTypesCreateDtoResponse>> CreateExaminationTypesAsync(ExaminationTypesCreateDtoRequest dto);
    }
}
