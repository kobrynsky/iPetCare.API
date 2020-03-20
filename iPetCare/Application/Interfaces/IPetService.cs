
using System;
using System.Threading.Tasks;
using Application.Dtos.Pet;
using Application.Services.Utilities;

namespace Application.Interfaces
{
    public interface IPetService
    {
        Task<ServiceResponse<PetsGetPetsDtoResponse>> GetPetsAsync();
        Task<ServiceResponse<PetsGetPetDtoResponse>> GetPetAsync(Guid petId);
        Task<ServiceResponse<PetsCreatePetDtoResponse>> CreatePetAsync(PetsCreatePetDtoRequest dto);
        Task<ServiceResponse<PetsUpdatePetDtoResponse>> UpdatePetAsync(Guid petId, PetsUpdatePetDtoRequest dto);
        Task<ServiceResponse> DeletePetAsync(Guid petId);
    }
}
