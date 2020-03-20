using System;
using System.ComponentModel.DataAnnotations;
using Domain.Models;

namespace Application.Dtos.Pet
{
    public class PetsGetPetDtoResponse
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
