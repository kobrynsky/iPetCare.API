using System.Threading.Tasks;
using Application.Dtos.Races;
using Application.Services.Utilities;

namespace Application.Interfaces
{
    public interface IRaceService
    {
        Task<ServiceResponse<CreateRaceDtoResponse>> CreateRaceAsync(CreateRaceDtoRequest dto);
        Task<ServiceResponse<GetAllRacesDtoResponse>> GetAllRacesAsync();
        Task<ServiceResponse<GetRaceDtoResponse>> GetRaceAsync(int raceId);
        Task<ServiceResponse<UpdateRaceDtoResponse>> UpdateRaceAsync(int raceId, UpdateRaceDtoRequest dto);
        Task<ServiceResponse<DeleteRaceDtoResponse>> DeleteRaceAsync(int raceId);
    }
}
