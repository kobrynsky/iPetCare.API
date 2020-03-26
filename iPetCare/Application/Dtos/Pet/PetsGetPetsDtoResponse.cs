using Domain.Models;
using System;
using System.Collections.Generic;

namespace Application.Dtos.Pet
{
    public class PetsGetPetsDtoResponse
    {
        public List<PetForPetsGetPetsDtoResponse> Pets { get; set; }
    }

    public class PetForPetsGetPetsDtoResponse
    {
        public Guid Id { get; set; }

        public string ImageUrl { get; set; }

        public string Name { get; set; }

        public float Weight { get; set; }

        public float Height { get; set; }

        public Gender Gender { get; set; }

        public DateTime BirthDate { get; set; }

        public string Race { get; set; }
    }
}