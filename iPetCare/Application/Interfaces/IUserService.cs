using System.Threading.Tasks;
using Application.Dtos.Users;
using Application.Dtos.Vets;
using Application.Services.Utilities;

namespace Application.Interfaces
{
    public interface IUserService
    {
        Task<ServiceResponse<LoginDtoResponse>> LoginAsync(LoginDtoRequest dto);
        Task<ServiceResponse<RegisterDtoResponse>> RegisterAsync(RegisterDtoRequest dto);
        Task<ServiceResponse<GetAllUsersDtoResponse>> GetAllAsync();
        Task<ServiceResponse<EditProfileDtoResponse>> EditProfileAsync(EditProfileDtoRequest dto);
        Task<ServiceResponse<GetVetsDtoResponse>> GetVetsAsync(GetVetsDtoRequest dto);
    }
}
