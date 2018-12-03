using System.ComponentModel.DataAnnotations;

namespace _101mngr.AuthorizationServer.Models
{
    public class LoginInputModel
    {
        [Required] [EmailAddress] public string Email { get; set; }

        [Required] public string Password { get; set; }
    }
}