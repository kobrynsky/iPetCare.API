using System.Threading.Tasks;
using Application.Dtos.Species;
using Application.Services.Utilities;


namespace Application.Interfaces
{
    public interface ISpeciesService
    {
        Task<ServiceResponse<CreateSpeciesDtoResponse>> CreateSpeciesAsync(CreateSpeciesDtoRequest dto);
        Task<ServiceResponse<GetAllSpeciesDtoResponse>> GetAllSpeciesAsync();
        Task<ServiceResponse<GetSpeciesDtoResponse>> GetSpeciesAsync(int speciesId);
        Task<ServiceResponse<UpdateSpeciesDtoResponse>> UpdateSpeciesAsync(int speciesId, UpdateSpeciesDtoRequest dto);
        Task<ServiceResponse<DeleteSpeciesDtoResponse>> DeleteSpeciesAsync(int speciesId);
    }
}
