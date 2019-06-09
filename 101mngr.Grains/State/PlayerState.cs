using System;
using System.Collections.Generic;
using _101mngr.Contracts.Enums;
using _101mngr.Contracts.Models;

namespace _101mngr.Grains
{
    public class PlayerState
    {
        public PlayerState()
        {
            MatchHistory = new List<MatchDto>();
        }

        public string Id { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public List<MatchDto> MatchHistory { get; set; }

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

        public void Apply(MatchPlayed @event)
        {
            MatchHistory.Add(new MatchDto
            {
                Id = @event.Id, Name = @event.Name, CreatedAt = @event.CreatedAt
            });
            Version++;
        }

        public void Apply(IPlayerEvent @event)
        {
            switch (@event)
            {
                case MatchPlayed matchPlayed:
                    Apply(matchPlayed);
                    break;
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

    public class PlayerLevelRaised : IPlayerEvent
    {
        public int LevelRaise { get; set; }
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

    public class MatchPlayed : IPlayerEvent
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public DateTime CreatedAt { get; set; }

        public string[] Players { get; set; }
    }
}