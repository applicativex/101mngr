using System;
using _101mngr.Contracts.Enums;

namespace _101mngr.Contracts.Models
{
    public class PlayerDto
    {   
        public long Id { get; set; }

        public long AccountId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime BirthDate { get; set; }

        public string CountryCode { get; set; }

        public double Height { get; set; }

        public double Weight { get; set; }

        public int Aggression { get; set; } 

        public int Experience { get; set; }

        public decimal WalletBalance { get; set; }

        public PlayerType PlayerType { get; set; }

        public int Level { get; set; }
    }
}