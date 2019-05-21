namespace _101mngr.Contracts.Models
{
    public class TrainingResultDto
    {
        public int TackleDelta { get; set; }

        public int CoverageDelta { get; set; }
            
        public int DribblingDelta { get; set; }

        public int ReceivingDelta { get; set; }

        public int PassingDelta { get; set; }

        public int EnduranceDelta { get; set; }

        public int HittingPowerDelta { get; set; }

        public int HittingAccuracyDelta { get; set; }
    }
}