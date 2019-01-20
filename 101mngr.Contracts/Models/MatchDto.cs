using System;

namespace _101mngr.Contracts.Models
{
    public class MatchDto
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public DateTime CreatedAt { get; set; }

        public string[] Players { get; set; } 
    }
}   