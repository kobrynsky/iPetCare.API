using Domain.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Pet
{
    public class PetsCreatePetDtoRequest
    {
        public Guid Id { get; set; }

        public string ImageUrl { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        public float Weight { get; set; }

        public float Height { get; set; }

        public Gender? Gender { get; set; }

        public DateTime BirthDate { get; set; }

        public int RaceId { get; set; }
    }
}