using System.Threading.Tasks;
using Application.Dtos.Races;
using Application.Services.Utilities;

namespace Application.Interfaces
{
    public interface IRaceService
    {
        Task<ServiceResponse<RaceCreateDtoResponse>> CreateAsync(RaceCreateDtoRequest dto);
        Task<ServiceResponse<RaceGetAllDtoResponse>> GetAllAsync();
        Task<ServiceResponse<RaceGetDtoResponse>> GetAsync(int raceId);
        Task<ServiceResponse<RaceUpdateDtoResponse>> UpdateAsync(int raceId, RaceUpdateDtoRequest dto);
        Task<ServiceResponse<RaceDeleteDtoResponse>> DeleteAsync(int raceId);
    }
}
