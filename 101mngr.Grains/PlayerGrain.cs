using System;
using System.Threading.Tasks;
using Orleans;
using _101mngr.Contracts;
using _101mngr.Contracts.Models;
using System.Linq;
using _101mngr.Leagues;

namespace _101mngr.Grains
{
    public class PlayerGrain : Grain, IPlayerGrain
    {
        private readonly LeagueService _leagueService;
        private readonly IEventStorage _eventStorage;
        private long PlayerId => this.GetPrimaryKeyLong();

        private TrainingResultDto CurrentTraining { get; set; }

        private PlayerState State { get; set; }

        public PlayerGrain(LeagueService leagueService, IEventStorage eventStorage)
        {
            _leagueService = leagueService;
            _eventStorage = eventStorage;
        }

        public override async Task OnActivateAsync()
        {
            State = (await _eventStorage.GetStreamState<PlayerState>(this.GetPrimaryKeyLong().ToString())).Value;
            await base.OnActivateAsync();
        }

        public Task<long> GetPlayer()
        {
            return Task.FromResult(PlayerId);
        }

        public async Task Create(CreatePlayerDto request)
        {
            await RaiseEvent(new PlayerCreated
            {
                Id = this.GetPrimaryKeyLong().ToString(), UserName = request.UserName, CountryCode = request.CountryCode,
                Email = request.Email
            });
        }

        public async Task ProfileInfo(ProfileInfoDto dto)
        {
            var profileInfoChanged = new ProfileInfoChanged
            {
                CountryCode = dto.CountryCode, PlayerType = dto.PlayerType, FirstName = dto.FirstName,
                LastName = dto.LastName, DateOfBirth = dto.DateOfBirth, Weight = dto.Weight, Height = dto.Height
            };
            await RaiseEvent(profileInfoChanged);
        }

        public Task<PlayerDto> GetPlayerInfo()
        {
            var result = new PlayerDto
            {
                Id = State.Id,
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

        public async Task<string> NewMatch(string matchName)
        {
            var matchId = Guid.NewGuid().ToString();
            var matchRoomGrain = GrainFactory.GetGrain<IMatchRoomGrain>(matchId);
            await matchRoomGrain.NewRoom(PlayerId, GetFullName(State.FirstName, State.LastName));
            return matchId;
        }

        public async Task AddMatchHistory(MatchDto match)
        {
            var matchPlayed = new MatchPlayed
            {
                Id = match.Id, Name = match.Name, CreatedAt = DateTime.UtcNow
            };
            await RaiseEvent(matchPlayed);
        }

        public async Task<string> RandomMatch()
        {
            var matchId = $"{PlayerId}:{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}";
            //var matchGrain = GrainFactory.GetGrain<IMatchGrain>(matchId);
            //await matchGrain.NewMatch(PlayerId, GetFullName(State.FirstName, State.LastName),
            //    $"Random Match {DateTimeOffset.UtcNow.ToUnixTimeSeconds()}");
            //var virtualPlayers = _leagueService.GetPlayers().Take(21).ToArray();
            //foreach (var virtualPlayer in virtualPlayers)
            //{
            //    // todo: handle id long vs string
            //    await matchGrain.JoinMatch(
            //        long.Parse(virtualPlayer.Id), GetFullName(virtualPlayer.FirstName, virtualPlayer.LastName), true);
            //}
            return matchId;
        }

        public Task StartTraining()
        {
            CurrentTraining = new TrainingResultDto();
            return Task.CompletedTask;
        }

        public async Task<TrainingResultDto> GetCurrentTraining()
        {
            return CurrentTraining != null
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

        public Task<MatchDto[]> GetMatchHistory()
        {
            return Task.FromResult(State.MatchHistory.OrderByDescending(x => x.CreatedAt).ToArray());
        }

        private static string GetFullName(string firstName, string lastName)
        {
            return $"{firstName} {lastName}";
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
