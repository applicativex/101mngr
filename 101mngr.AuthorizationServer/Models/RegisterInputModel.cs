using System.ComponentModel.DataAnnotations;

namespace _101mngr.AuthorizationServer.Models
{
    public class RegisterInputModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
    }
}