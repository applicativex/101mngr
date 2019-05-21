namespace _101mngr.Contracts.Models
{
    public class AcquiredSkillsDto
    {
        public long PlayerId { get; set; }  

        public int Tackle { get; set; }

        public int Coverage { get; set; }

        public int Dribbling { get; set; }

        public int Receiving { get; set; }

        public int Passing { get; set; }

        public int Endurance { get; set; }

        public int HittingPower { get; set; }

        public int HittingAccuracy { get; set; }
    }
}