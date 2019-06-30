using System;
using System.Threading.Tasks;
using Orleans;
using _101mngr.Contracts;
using _101mngr.Contracts.Models;
using _101mngr.Contracts.Enums;

namespace _101mngr.Grains
{
    public class PlayerGrain : Grain, IPlayerGrain
    {
        private readonly IEventStorage _eventStorage;

        private TrainingResultDto CurrentTraining { get; set; }

        private PlayerState State { get; set; }

        public PlayerGrain(IEventStorage eventStorage)
        {
            _eventStorage = eventStorage;
        }

        public override async Task OnActivateAsync()
        {
            State = (await _eventStorage.GetStreamState<PlayerState>(this.GetPrimaryKeyLong().ToString())).Value;
            await base.OnActivateAsync();
        }

        public async Task Create(string userName, string email)
        {
            await RaiseEvent(new PlayerCreated
            {
                Id = this.GetPrimaryKeyLong().ToString(), UserName = userName, Email = email
            });
        }

        public async Task UpdateProfileInfo(string firstName, string lastName, DateTime dateOfBirth, string countryCode, PlayerType playerType, double height, double weight)
        {
            var profileInfoChanged = new ProfileInfoChanged
            {
                CountryCode = countryCode, PlayerType = playerType, FirstName = firstName,
                LastName = lastName, DateOfBirth = dateOfBirth, Weight = weight, Height = height
            };
            await RaiseEvent(profileInfoChanged);
        }

        public Task<PlayerDto> GetPlayerInfo()
        {
            var result = new PlayerDto
            {
                Id = long.Parse(State.Id),
                FirstName = State.FirstName,
                LastName = State.LastName,
                BirthDate = State.DateOfBirth,
                Height = State.Height,
                Weight = State.Weight,
                CountryCode = State.CountryCode,
                Level = State.Level,
                PlayerType = State.PlayerType
            };
            return Task.FromResult(result);
        }

        public Task StartTraining()
        {
            CurrentTraining = new TrainingResultDto();
            return Task.CompletedTask;
        }

        public Task<TrainingResultDto> GetCurrentTraining()
        {
            var result = CurrentTraining != null
                ? new TrainingResultDto()
                {
                    CoverageDelta = CurrentTraining.CoverageDelta,
                    DribblingDelta = CurrentTraining.DribblingDelta,
                    EnduranceDelta = CurrentTraining.EnduranceDelta,
                    HittingAccuracyDelta = CurrentTraining.HittingAccuracyDelta,
                    HittingPowerDelta = CurrentTraining.HittingPowerDelta,
                    PassingDelta = CurrentTraining.PassingDelta,
                    ReceivingDelta = CurrentTraining.ReceivingDelta,
                    TackleDelta = CurrentTraining.TackleDelta
                }
                : new TrainingResultDto();
            return Task.FromResult(result);
        }

        public Task<TrainingResultDto> TrainPassing()
        {
            if (CurrentTraining == null)
            {
                CurrentTraining = new TrainingResultDto();
            }
            CurrentTraining.PassingDelta++;
            return Task.FromResult(new TrainingResultDto
            {
                CoverageDelta = CurrentTraining.CoverageDelta,
                DribblingDelta = CurrentTraining.DribblingDelta,
                EnduranceDelta = CurrentTraining.EnduranceDelta,
                HittingAccuracyDelta = CurrentTraining.HittingAccuracyDelta,
                HittingPowerDelta = CurrentTraining.HittingPowerDelta,
                PassingDelta = CurrentTraining.PassingDelta,
                ReceivingDelta = CurrentTraining.ReceivingDelta,
                TackleDelta = CurrentTraining.TackleDelta
            });
        }

        public Task<TrainingResultDto> TrainEndurance()
        {
            if (CurrentTraining == null)
            {
                CurrentTraining = new TrainingResultDto();
            }
            CurrentTraining.EnduranceDelta++;
            return Task.FromResult(new TrainingResultDto()
            {
                CoverageDelta = CurrentTraining.CoverageDelta,
                DribblingDelta = CurrentTraining.DribblingDelta,
                EnduranceDelta = CurrentTraining.EnduranceDelta,
                HittingAccuracyDelta = CurrentTraining.HittingAccuracyDelta,
                HittingPowerDelta = CurrentTraining.HittingPowerDelta,
                PassingDelta = CurrentTraining.PassingDelta,
                ReceivingDelta = CurrentTraining.ReceivingDelta,
                TackleDelta = CurrentTraining.TackleDelta
            });
        }

        public Task<TrainingResultDto> TrainDribbling()
        {
            if (CurrentTraining == null)
            {
                CurrentTraining = new TrainingResultDto();
            }
            CurrentTraining.DribblingDelta++;
            return Task.FromResult(new TrainingResultDto()
            {
                CoverageDelta = CurrentTraining.CoverageDelta,
                DribblingDelta = CurrentTraining.DribblingDelta,
                EnduranceDelta = CurrentTraining.EnduranceDelta,
                HittingAccuracyDelta = CurrentTraining.HittingAccuracyDelta,
                HittingPowerDelta = CurrentTraining.HittingPowerDelta,
                PassingDelta = CurrentTraining.PassingDelta,
                ReceivingDelta = CurrentTraining.ReceivingDelta,
                TackleDelta = CurrentTraining.TackleDelta
            });
        }

        public Task<TrainingResultDto> TrainCoverage()
        {
            if (CurrentTraining == null)
            {
                CurrentTraining = new TrainingResultDto();
            }
            CurrentTraining.CoverageDelta++;
            return Task.FromResult(new TrainingResultDto()
            {
                CoverageDelta = CurrentTraining.CoverageDelta,
                DribblingDelta = CurrentTraining.DribblingDelta,
                EnduranceDelta = CurrentTraining.EnduranceDelta,
                HittingAccuracyDelta = CurrentTraining.HittingAccuracyDelta,
                HittingPowerDelta = CurrentTraining.HittingPowerDelta,
                PassingDelta = CurrentTraining.PassingDelta,
                ReceivingDelta = CurrentTraining.ReceivingDelta,
                TackleDelta = CurrentTraining.TackleDelta
            });
        }

        public async Task FinishTraining()
        {
            if (CurrentTraining == null)
            {
                CurrentTraining = new TrainingResultDto();
            }
            var playerAcquiredSkillsChanged = new PlayerAcquiredSkillsChanged
            {
                CoverageDelta = CurrentTraining.CoverageDelta,
                DribblingDelta = CurrentTraining.DribblingDelta,
                EnduranceDelta = CurrentTraining.EnduranceDelta,
                HittingAccuracyDelta = CurrentTraining.HittingAccuracyDelta,
                HittingPowerDelta = CurrentTraining.HittingPowerDelta,
                PassingDelta = CurrentTraining.PassingDelta,
                ReceivingDelta = CurrentTraining.ReceivingDelta,
                TackleDelta = CurrentTraining.TackleDelta
            };
            await RaiseEvent(playerAcquiredSkillsChanged);
            CurrentTraining = null;
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
