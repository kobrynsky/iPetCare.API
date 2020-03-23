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
        Task<ServiceResponse<SpeciesCreateSpeciesDtoResponse>> CreateAsync(SpeciesCreateSpeciesDtoRequest dto);
        Task<ServiceResponse<SpeciesGetAllSpeciesDtoResponse>> GetAllAsync();
        Task<ServiceResponse<SpeciesGetSpeciesDtoResponse>> GetAsync(int speciesId);
        Task<ServiceResponse<SpeciesUpdateSpeciesDtoResponse>> UpdateAsync(int speciesId, SpeciesUpdateSpeciesDtoRequest dto);
        Task<ServiceResponse<SpeciesDeleteSpeciesDtoResponse>> DeleteAsync(int speciesId);
    }
}
