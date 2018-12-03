using System;
using System.ComponentModel.DataAnnotations;

namespace _101mngr.AuthorizationServer.Models
{
    public class AddPlayerInputModel
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public DateTime BirthDate { get; set; }

        [Required]
        public string CountryCode { get; set; }

        [Required]
        public double Height { get; set; }

        [Required]
        public double Weight { get; set; }

        [Required]
        public int Aggression { get; set; }
    }
}