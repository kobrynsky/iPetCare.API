using System.Threading.Tasks;
using Application.Dtos.Users;
using Application.Services.Utilities;

namespace Application.Interfaces
{
    public interface IUserService
    {
        Task<ServiceResponse<LoginDtoResponse>> LoginAsync(LoginDtoRequest dto);
        Task<ServiceResponse<RegisterDtoResponse>> RegisterAsync(RegisterDtoRequest dto);
    }
}
