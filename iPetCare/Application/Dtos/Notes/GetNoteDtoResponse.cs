using Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Dtos.Notes
{
    public class GetNoteDtoResponse
    {
        public Guid Id { get; set; }
        public string Payload { get; set; }
        public DateTime CreatedAt { get; set; }
        public PetForGetNoteDtoResponse Pet { get; set; }
        public UserForGetNoteDtoResponse User { get; set; }
    }

    public class PetForGetNoteDtoResponse
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

    public class UserForGetNoteDtoResponse
    {
        public string Id { get; set; }
        public string Role { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string ImageUrl { get; set; }
        public string Specialization { get; set; }
        public string PlaceOfResidence { get; set; }
    }
}
