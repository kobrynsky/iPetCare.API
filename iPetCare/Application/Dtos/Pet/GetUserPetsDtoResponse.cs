using System;
using System.Collections.Generic;
using System.Text;
using Domain.Models;

namespace Application.Dtos.Pet
{
    public class GetUserPetsDtoResponse
    {
        public List<PetForGetUserPetsDtoResponse> Pets { get; set; }
    }

    public class PetForGetUserPetsDtoResponse
    {
        public Guid Id { get; set; }
        public string ImageUrl { get; set; }
        public string Name { get; set; }
        public float Weight { get; set; }
        public float Height { get; set; }
        public Gender Gender { get; set; }
        public DateTime BirthDate { get; set; }
        public string Race { get; set; }
        public string Species { get; set; }
        public bool? InvitationStatus { get; set; }
    }
}
