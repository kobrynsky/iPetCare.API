using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Application.Dtos.Users
{
    public class EditProfileDtoRequest
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public string Email { get; set; }

        public IFormFile Image { get; set; }

        public string Specialization { get; set; }

        public string PlaceOfResidence { get; set; }
    }
}
