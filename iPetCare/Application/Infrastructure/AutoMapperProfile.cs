using Application.Dtos.Pet;
using Application.Dtos.Users;
using AutoMapper;
using Domain.Models;

namespace Application.Infrastructure
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            MapsForUser();
            MapsForPets();
        }

        private void MapsForUser()
        {
            CreateMap<ApplicationUser, UserGetAllDtoResponse>();
        }

        private void MapsForPets()
        {
            CreateMap<Pet, PetForPetsGetPetsDtoResponse>()
                .ForMember(d => d.Race, opt => opt.MapFrom(s => s.Race.Name));
            CreateMap<PetsCreatePetDtoRequest, Pet>();
            CreateMap<Pet, PetsCreatePetDtoResponse>()
                .ForMember(d => d.Race, opt => opt.MapFrom(s => s.Race.Name));
            CreateMap<Pet, PetsGetPetDtoResponse>()
                .ForMember(d => d.Race, opt => opt.MapFrom(s => s.Race.Name));
            CreateMap<PetsUpdatePetDtoRequest, PetsUpdatePetDtoResponse>();
            CreateMap<PetsUpdatePetDtoRequest, Pet>();
        }
    }
}
