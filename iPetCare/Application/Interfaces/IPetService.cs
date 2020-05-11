
using System;
using System.Threading.Tasks;
using Application.Dtos.Pet;
using Application.Services.Utilities;

namespace Application.Interfaces
{
    public interface IPetService
    {
        Task<ServiceResponse<GetPetsDtoResponse>> GetPetsAsync();
        Task<ServiceResponse<GetPetDtoResponse>> GetPetAsync(Guid petId);
        Task<ServiceResponse<CreatePetDtoResponse>> CreatePetAsync(CreatePetDtoRequest dto);
        Task<ServiceResponse<UpdatePetDtoResponse>> UpdatePetAsync(Guid petId, UpdatePetDtoRequest dto);
        Task<ServiceResponse> DeletePetAsync(Guid petId);
        Task<ServiceResponse<GetMyPetsDtoResponse>> GetMyPetsAsync();
        Task<ServiceResponse<GetSharedPetsDtoResponse>> GetSharedPetsAsync();
        Task<ServiceResponse<GetUserPetsDtoResponse>> GetUserPetsAsync(string userId);
        Task<ServiceResponse<GetInvitationsStatusDtoResponse>> GetInvitationsStatusAsync(Guid petId);
        Task<ServiceResponse<GetInvitationsStatusDtoResponse>> GetInvitationsStatusAsync();
    }
}
