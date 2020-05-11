using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Application.Dtos.Users
{
    public class EditProfileDtoRequest
    {
        [Required(ErrorMessage = "Pole jest wymagane")]
        [MaxLength(255, ErrorMessage = "Długość nie może być większa, niż 255 znaków")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Pole jest wymagane")]
        [MaxLength(255, ErrorMessage = "Długość nie może być większa, niż 255 znaków")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Pole jest wymagane")]
        [MaxLength(255, ErrorMessage = "Długość nie może być większa, niż 255 znaków")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Pole jest wymagane")]
        [MaxLength(255, ErrorMessage = "Długość nie może być większa, niż 255 znaków")]
        public string Email { get; set; }

        public IFormFile Image { get; set; }

        [MaxLength(255, ErrorMessage = "Długość nie może być większa, niż 255 znaków")]
        public string Specialization { get; set; }

        [MaxLength(255, ErrorMessage = "Długość nie może być większa, niż 255 znaków")]
        public string PlaceOfResidence { get; set; }
    }
}
