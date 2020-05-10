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

        [Required(ErrorMessage = "Pole jest wymagane")]
        [MaxLength(255, ErrorMessage = "Długość nie może być większa, niż 255 znaków")]
        public string Name { get; set; }
    }
}