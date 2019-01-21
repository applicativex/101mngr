using System;
using _101mngr.Contracts.Enums;

namespace _101mngr.Contracts.Models
{
    public class ProfileInfoDto
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string CountryCode { get; set; } 

        public double Height { get; set; }

        public double Weight { get; set; }

        public PlayerType PlayerType { get; set; }  
    }
}