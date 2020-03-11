using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Users
{
    public class LoginDtoRequest
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
