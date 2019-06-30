using System;

namespace _101mngr.Contracts.Models
{
    public class PlayerDto
    {   
        public long Id { get; set; }
       
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime BirthDate { get; set; }

        public string CountryCode { get; set; }

        public double Height { get; set; }

        public double Weight { get; set; }

        public int PlayerType { get; set; }

        public AcquiredSkillsDto AcquiredSkills { get; set; }   
    }
}