﻿using Domain.Models;
using System;

namespace Application.Dtos.Pet
{
    public class UpdatePetDtoRequest
    {
        public string ImageUrl { get; set; }
        public string Name { get; set; }
        public float Weight { get; set; }
        public float Height { get; set; }
        public Gender? Gender { get; set; }
        public DateTime BirthDate { get; set; }
        public int RaceId { get; set; }
    }
}