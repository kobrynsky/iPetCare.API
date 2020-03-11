using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Application.Dtos.Users;
using Application.Services.Utilities;

namespace Application.Interfaces
{
    public interface IUserService
    {
        Task<ServiceResponse<LoginDtoResponse>> LoginAsync(LoginDtoRequest dto);
    }
}
