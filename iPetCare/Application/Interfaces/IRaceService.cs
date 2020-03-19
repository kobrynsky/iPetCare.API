using System.Threading.Tasks;
using Application.Dtos.Races;
using Application.Services.Utilities;

namespace Application.Interfaces
{
    public interface IRaceService
    {
        Task<ServiceResponse<CreateDtoResponse>> CreateAsync(CreateDtoRequest dto);
        Task<ServiceResponse<GetAllDtoResponse>> GetAllAsync();
        Task<ServiceResponse<GetDtoResponse>> GetAsync(int raceId);
        Task<ServiceResponse<PutDtoResponse>> PutAsync(int raceId, PutDtoRequest dto);
    }
}
