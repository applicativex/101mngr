using System;
using System.Threading.Tasks;
using Orleans;
using _101mngr.Contracts;
using _101mngr.Contracts.Models;
using System.Collections.Generic;
using System.Linq;
using _101mngr.Contracts.Enums;
using _101mngr.Leagues;

namespace _101mngr.Grains
{
    public class PlayerGrain : Grain, IPlayerGrain
    {
        private readonly LeagueService _leagueService;
        private readonly IEventStorage _eventStorage;
        private long PlayerId => this.GetPrimaryKeyLong();

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

        public async Task AddMatchHistory(MatchDto match)
        {
            var matchPlayed = new MatchPlayed
            {
                Id = match.Id, Name = match.Name, Players = match.Players, CreatedAt = DateTime.UtcNow
            };
            await RaiseEvent(matchPlayed);
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

        private async Task RaiseEvent(IPlayerEvent @event)
        {
            await _eventStorage.AppendToStream(this.GetPrimaryKeyLong().ToString(), State.Version + 1, @event);
            State.Apply(@event);
        }
    }

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
                {Id = @event.Id, Name = @event.Name, Players = @event.Players, CreatedAt = @event.CreatedAt});
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
                default:
                    throw new NotSupportedException();
            }
        }
    }

    public interface IPlayerEvent
    {

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
