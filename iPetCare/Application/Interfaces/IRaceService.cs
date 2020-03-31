using System.Threading.Tasks;
using Application.Dtos.Races;
using Application.Services.Utilities;

namespace Application.Interfaces
{
    public interface IRaceService
    {
        Task<ServiceResponse<CreateRaceDtoResponse>> CreateAsync(CreateRaceDtoRequest dto);
        Task<ServiceResponse<GetAllRacesDtoResponse>> GetAllAsync();
        Task<ServiceResponse<GetRaceDtoResponse>> GetAsync(int raceId);
        Task<ServiceResponse<UpdateRaceDtoResponse>> UpdateAsync(int raceId, UpdateRaceDtoRequest dto);
        Task<ServiceResponse<DeleteRaceDtoResponse>> DeleteAsync(int raceId);
    }
}
