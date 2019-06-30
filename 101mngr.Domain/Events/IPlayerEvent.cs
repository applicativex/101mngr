using System;
using _101mngr.Domain.Enums;

namespace _101mngr.Domain.Events
{
    public interface IPlayerEvent
    {
    }

    /// <summary>
    /// 
    /// </summary>
    public class PlayerCreated : IPlayerEvent
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string CountryCode { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class ProfileInfoChanged : IPlayerEvent
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string CountryCode { get; set; }

        public double Height { get; set; }

        public double Weight { get; set; }

        public PlayerType PlayerType { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class PlayerAcquiredSkillsChanged : IPlayerEvent
    {
        public static PlayerAcquiredSkillsChanged From(Training training)=> new PlayerAcquiredSkillsChanged
        {
            CoverageDelta = training.CoverageDelta,
            DribblingDelta = training.DribblingDelta,
            EnduranceDelta = training.EnduranceDelta,
            HittingAccuracyDelta = training.HittingAccuracyDelta,
            HittingPowerDelta = training.HittingPowerDelta,
            PassingDelta = training.PassingDelta,
            ReceivingDelta = training.ReceivingDelta,
            TackleDelta = training.TackleDelta
        };

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