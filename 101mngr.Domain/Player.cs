using System;
using _101mngr.Domain.Enums;
using _101mngr.Domain.Events;

namespace _101mngr.Domain
{
    public class Player
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

        public Training CurrentTraining { get; private set; }   

        public PlayerType PlayerType { get; set; }

        public AcquiredSkills AcquiredSkills { get; set; } = new AcquiredSkills();

        public int Version { get; set; }

        public void ResetTraining()
        {
            CurrentTraining = new Training();
        }

        public void TrainPassing()
        {
            CurrentTraining = CurrentTraining.Passing();
        }

        public void TrainTackle()
        {
            CurrentTraining = CurrentTraining.Tackle();
        }

        public void TrainCoverage()
        {
            CurrentTraining = CurrentTraining.Coverage();
        }

        public void TrainEndurance()
        {
            CurrentTraining = CurrentTraining.Endurance();
        }

        public void TrainDribbling()
        {
            CurrentTraining = CurrentTraining.Dribbling();
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

        public void Apply(PlayerAcquiredSkillsChanged @event)
        {
            AcquiredSkills.Apply(@event);
            Version++;
        }
    }
}