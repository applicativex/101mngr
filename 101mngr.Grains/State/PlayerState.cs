using System;
using _101mngr.Contracts.Enums;

namespace _101mngr.Grains
{
    public class PlayerState
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string CountryCode { get; set; }

        public double Height { get; set; }

        public double Weight { get; set; }

        public int Level { get; set; }

        public PlayerType PlayerType { get; set; }

        public AcquiredSkills AcquiredSkills { get; set; } = new AcquiredSkills();

        public int Version { get; set; }    

        public void Apply(PlayerCreated @event)
        {
            Id = @event.Id;
            UserName = @event.UserName;
            Email = @event.Email;
            CountryCode = @event.CountryCode;
            Version++;
        }

        public void Apply(ProfileInfoChanged @event)
        {
            FirstName = @event.FirstName;
            LastName = @event.LastName;
            DateOfBirth = @event.DateOfBirth;
            CountryCode = @event.CountryCode;
            Height = @event.Height;
            Weight = @event.Weight;
            PlayerType = @event.PlayerType;
            Version++;
        }

        public void Apply(IPlayerEvent @event)
        {
            switch (@event)
            {
                case PlayerCreated playerCreated:
                    Apply(playerCreated);
                    break;
                case ProfileInfoChanged profileInfoChanged:
                    Apply(profileInfoChanged);
                    break;
                case PlayerAcquiredSkillsChanged playerAcquiredSkillsChanged:
                    Apply(playerAcquiredSkillsChanged);
                    break;
                default:
                    throw new NotSupportedException();
            }
        }

        public void Apply(PlayerAcquiredSkillsChanged @event)
        {
            AcquiredSkills.Apply(@event);
            Version++;
        }
    }

    public interface IPlayerEvent
    {

    }

    public class PlayerAcquiredSkillsChanged : IPlayerEvent
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

    public class PlayerCreated : IPlayerEvent
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string CountryCode { get; set; }
    }

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
}