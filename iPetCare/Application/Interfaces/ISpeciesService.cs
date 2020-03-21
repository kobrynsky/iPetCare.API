using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Application.Dtos.Species;
using Application.Services.Utilities;


namespace Application.Interfaces
{
    public interface ISpeciesService
    {
        Task<ServiceResponse<SpeciesCreateDtoResponse>> CreateAsync(SpeciesCreateDtoRequest dto);
        Task<ServiceResponse<SpeciesGetAllDtoResponse>> GetAllAsync();
        Task<ServiceResponse<SpeciesGetDtoResponse>> GetAsync(int speciesId);
        Task<ServiceResponse<SpeciesUpdateDtoResponse>> UpdateAsync(int speciesId, SpeciesUpdateDtoRequest dto);
        Task<ServiceResponse<SpeciesDeleteDtoResponse>> DeleteAsync(int speciesId);
    }
}
