using System;
using System.Threading.Tasks;
using Orleans;
using _101mngr.Contracts;
using _101mngr.Contracts.Models;
using _101mngr.Domain;
using _101mngr.Domain.Abstractions;
using _101mngr.Domain.Enums;
using _101mngr.Domain.Events;

namespace _101mngr.Grains
{
    public class PlayerGrain : Grain, IPlayerGrain
    {
        private readonly IEventStorage _eventStorage;

        private Player State { get; set; }  

        public PlayerGrain(IEventStorage eventStorage)
        {
            _eventStorage = eventStorage;
        }

        public override async Task OnActivateAsync()
        {
            State = (await _eventStorage.GetStreamState<Player>(this.GetPrimaryKeyLong().ToString())).Value;
            await base.OnActivateAsync();
        }

        public async Task Create(string userName, string email)
        {
            await RaiseEvent(new PlayerCreated
            {
                Id = this.GetPrimaryKeyLong().ToString(), UserName = userName, Email = email
            });
        }

        public async Task UpdateProfileInfo(string firstName, string lastName, DateTime dateOfBirth, string countryCode, int playerType, double height, double weight)
        {
            var profileInfoChanged = new ProfileInfoChanged
            {
                CountryCode = countryCode, PlayerType = (PlayerType)playerType, FirstName = firstName,
                LastName = lastName, DateOfBirth = dateOfBirth, Weight = weight, Height = height
            };
            await RaiseEvent(profileInfoChanged);
        }

        public Task<PlayerDto> GetPlayerInfo()
        {
            return Task.FromResult(State.ToDto());
        }

        public Task StartTraining()
        {
            State.ResetTraining();
            return Task.CompletedTask;
        }

        public async Task FinishTraining()
        {
            await RaiseEvent(PlayerAcquiredSkillsChanged.From(State.CurrentTraining));
            State.ResetTraining();
        }

        public Task<TrainingDto> GetCurrentTraining()
        {
            return Task.FromResult(State.CurrentTraining.ToDto());
        }

        public Task<TrainingDto> TrainPassing()
        {
            State.TrainPassing();
            return Task.FromResult(State.CurrentTraining.ToDto());
        }

        public Task<TrainingDto> TrainEndurance()
        {
            State.TrainEndurance();
            return Task.FromResult(State.CurrentTraining.ToDto());
        }

        public Task<TrainingDto> TrainDribbling()
        {
            State.TrainDribbling();
            return Task.FromResult(State.CurrentTraining.ToDto());
        }

        public Task<TrainingDto> TrainCoverage()
        {
            State.TrainCoverage();
            return Task.FromResult(State.CurrentTraining.ToDto());
        }

        private async Task RaiseEvent(IPlayerEvent @event)
        {
            try
            {
                await _eventStorage.AppendToStream(this.GetPrimaryKeyLong().ToString(), State.Version + 1, @event);
                State.Apply(@event);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
