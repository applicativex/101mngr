using System;
using System.Threading.Tasks;
using Orleans;
using _101mngr.Contracts;
using _101mngr.Contracts.Models;
using System.Collections.Generic;
using System.Linq;
using Orleans.Providers;
using _101mngr.Contracts.Enums;
using _101mngr.Leagues;

namespace _101mngr.Grains
{
    [StorageProvider(ProviderName = "OrleansStorage")]
    public class PlayerGrain : Grain<PlayerState>, IPlayerGrain
    {
        private readonly LeagueService _leagueService;
        private long PlayerId => this.GetPrimaryKeyLong();

        public PlayerGrain(LeagueService leagueService)
        {
            _leagueService = leagueService;
        }
        
        public Task<long> GetPlayer()
        {
            return Task.FromResult(PlayerId);
        }

        public Task Create(CreatePlayerDto request)
        {
            State = new PlayerState
            {
                Id = PlayerId,
                UserName = request.UserName,
                Email = request.Email,
                CountryCode = request.CountryCode,
                MatchHistory = new List<MatchDto>()
            };
            return WriteStateAsync();
        }

        public Task ProfileInfo(ProfileInfoDto dto)
        {
            State.FirstName = dto.FirstName;
            State.LastName = dto.LastName;
            State.DateOfBirth = dto.DateOfBirth;
            State.CountryCode = dto.CountryCode;
            State.Height = dto.Height;
            State.Weight = dto.Weight;
            State.PlayerType = dto.PlayerType;
            return WriteStateAsync();
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
            var matchId = $"{PlayerId}:{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}";
            var matchGrain = GrainFactory.GetGrain<IMatchGrain>(matchId);
            await matchGrain.NewMatch(PlayerId, GetFullName(State.FirstName, State.LastName), matchName);
            return matchId;
        }

        public async Task JoinMatch(string matchId)
        {
            var matchGrain = GrainFactory.GetGrain<IMatchGrain>(matchId);
            await matchGrain.JoinMatch(PlayerId, GetFullName(State.FirstName, State.LastName), false);
        }

        public async Task LeaveMatch(string matchId)
        {
            var matchGrain = GrainFactory.GetGrain<IMatchGrain>(matchId);
            await matchGrain.LeaveMatch(PlayerId);
        }

        public Task AddMatchHistory(MatchDto match)
        {
            State.MatchHistory.Add(match);
            return WriteStateAsync();
        }

        public async Task<string> RandomMatch()
        {
            var matchId = $"{PlayerId}:{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}";
            var matchGrain = GrainFactory.GetGrain<IMatchGrain>(matchId);
            await matchGrain.NewMatch(PlayerId, GetFullName(State.FirstName, State.LastName),
                $"Random Match {DateTimeOffset.UtcNow.ToUnixTimeSeconds()}");
            var virtualPlayers = _leagueService.GetPlayers().Take(21).ToArray();
            foreach (var virtualPlayer in virtualPlayers)
            {
                // todo: handle id long vs string
                await matchGrain.JoinMatch(
                    long.Parse(virtualPlayer.Id), GetFullName(virtualPlayer.FirstName, virtualPlayer.LastName), true);
            }

            await matchGrain.PlayMatch();
            return matchId;
        }

        public Task<MatchDto[]> GetMatchHistory()
        {
            return Task.FromResult(State.MatchHistory.OrderByDescending(x => x.CreatedAt).ToArray());
        }

        private static string GetFullName(string firstName, string lastName)
        {
            return $"{firstName} {lastName}";
        }
    }

    public class PlayerState
    {
        public long Id { get; set; }

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
    }
}
