using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Users
{
    public class LoginDtoRequest
    {
        [Required(ErrorMessage = "Pole jest wymagane")]
        [MaxLength(255, ErrorMessage = "Długość nie może być większa, niż 255 znaków")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Pole jest wymagane")]
        [MaxLength(255, ErrorMessage = "Długość nie może być większa, niż 255 znaków")]
        public string Password { get; set; }
    }
}
