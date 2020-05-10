using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Users
{
    public class RegisterDtoRequest
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

        [MaxLength(255, ErrorMessage = "Długość nie może być większa, niż 255 znaków")]
        [Required(ErrorMessage = "Pole jest wymagane")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Pole jest wymagane")]
        [MaxLength(255, ErrorMessage = "Długość nie może być większa, niż 255 znaków")]
        public string Role { get; set; }
    }
}
