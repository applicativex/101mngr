using System;
using _101mngr.Domain.Enums;

namespace _101mngr.WebApp.Models.Requests
{
    public class ProfileInfoInputModel
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string CountryCode { get; set; }

        public PlayerType PlayerType { get; set; }  

        public double Weight { get; set; }

        public double Height { get; set; }
    }
}