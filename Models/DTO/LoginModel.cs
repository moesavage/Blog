using System.ComponentModel.DataAnnotations;

namespace Blog.Models.DTO
{
    public class LoginModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        public bool KeepLoggedIn { get; set; }
    }
}
