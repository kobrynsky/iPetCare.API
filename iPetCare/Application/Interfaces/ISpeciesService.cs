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
        Task<ServiceResponse<CreateSpeciesDtoResponse>> CreateAsync(CreateSpeciesDtoRequest dto);
        Task<ServiceResponse<GetAllSpeciesDtoResponse>> GetAllAsync();
        Task<ServiceResponse<GetSpeciesDtoResponse>> GetAsync(int speciesId);
        Task<ServiceResponse<UpdateSpeciesDtoResponse>> UpdateAsync(int speciesId, UpdateSpeciesDtoRequest dto);
        Task<ServiceResponse<DeleteSpeciesDtoResponse>> DeleteAsync(int speciesId);
    }
}
