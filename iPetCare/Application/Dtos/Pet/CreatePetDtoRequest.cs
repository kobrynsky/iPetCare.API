using Domain.Models;
using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Application.Dtos.Pet
{
    public class CreatePetDtoRequest
    {
        public Guid Id { get; set; }
        public IFormFile Image { get; set; }
        public float Weight { get; set; }
        public float Height { get; set; }
        public Gender? Gender { get; set; }
        public DateTime BirthDate { get; set; }
        public int RaceId { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }
    }
}