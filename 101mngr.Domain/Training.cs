namespace _101mngr.Domain
{
    public struct Training
    {
        private Training(int tackleDelta, int coverageDelta, int dribblingDelta, int receivingDelta, int passingDelta, int enduranceDelta, int hittingPowerDelta, int hittingAccuracyDelta)
        {
            TackleDelta = tackleDelta;
            CoverageDelta = coverageDelta;
            DribblingDelta = dribblingDelta;
            ReceivingDelta = receivingDelta;
            PassingDelta = passingDelta;
            EnduranceDelta = enduranceDelta;
            HittingPowerDelta = hittingPowerDelta;
            HittingAccuracyDelta = hittingAccuracyDelta;
        }

        public int TackleDelta { get; }

        public int CoverageDelta { get; }

        public int DribblingDelta { get; }

        public int ReceivingDelta { get; }

        public int PassingDelta { get; }

        public int EnduranceDelta { get; }

        public int HittingPowerDelta { get; }

        public int HittingAccuracyDelta { get; }

        public Training Tackle() => new Training(
            TackleDelta + 1, CoverageDelta, DribblingDelta, ReceivingDelta, PassingDelta, EnduranceDelta,
            HittingPowerDelta,
            HittingAccuracyDelta);

        public Training Coverage() => new Training(
            TackleDelta, CoverageDelta + 1, DribblingDelta, ReceivingDelta, PassingDelta, EnduranceDelta,
            HittingPowerDelta,
            HittingAccuracyDelta);

        public Training Dribbling() => new Training(
            TackleDelta, CoverageDelta, DribblingDelta + 1, ReceivingDelta, PassingDelta, EnduranceDelta,
            HittingPowerDelta,
            HittingAccuracyDelta);

        public Training Receiving() => new Training(
            TackleDelta, CoverageDelta, DribblingDelta, ReceivingDelta + 1, PassingDelta, EnduranceDelta,
            HittingPowerDelta,
            HittingAccuracyDelta);

        public Training Passing() => new Training(
            TackleDelta, CoverageDelta, DribblingDelta, ReceivingDelta, PassingDelta + 1, EnduranceDelta,
            HittingPowerDelta,
            HittingAccuracyDelta);

        public Training Endurance() => new Training(
            TackleDelta, CoverageDelta, DribblingDelta, ReceivingDelta, PassingDelta, EnduranceDelta + 1,
            HittingPowerDelta,
            HittingAccuracyDelta);

        public Training HittingPower() => new Training(
            TackleDelta, CoverageDelta, DribblingDelta, ReceivingDelta, PassingDelta, EnduranceDelta,
            HittingPowerDelta + 1,
            HittingAccuracyDelta);

        public Training HittingAccuracy() => new Training(
            TackleDelta, CoverageDelta, DribblingDelta, ReceivingDelta, PassingDelta, EnduranceDelta,
            HittingPowerDelta,
            HittingAccuracyDelta + 1);
    }
}