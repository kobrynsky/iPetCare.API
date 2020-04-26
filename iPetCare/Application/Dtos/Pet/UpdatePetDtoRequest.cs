using Domain.Models;
using System;
using Microsoft.AspNetCore.Http;

namespace Application.Dtos.Pet
{
    public class UpdatePetDtoRequest
    {
        public IFormFile Image { get; set; }
        public string Name { get; set; }
        public float Weight { get; set; }
        public float Height { get; set; }
        public Gender? Gender { get; set; }
        public DateTime BirthDate { get; set; }
        public int RaceId { get; set; }
    }
}