using System;
using System.Collections.Generic;
using System.Text;
using Domain.Models;

namespace Application.Dtos.Pet
{
    public class PetsGetSharedPetsDtoResponse
    {
        public List<PetForPetsGetSharedPetsDtoResponse> Pets { get; set; }
    }

    public class PetForPetsGetSharedPetsDtoResponse
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
