using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Orleans;
using _101mngr.Contracts;
using _101mngr.WebApp.Data;
using PlayerType = _101mngr.WebApp.Services.PlayerType;

namespace _101mngr.WebApp.Controllers
{
    [Route("api/[controller]")]
    public class MatchController : Controller    
    {
        private readonly ApplicationDbContext _context;
        private readonly MatchRepository _matchRepository;
        private readonly IClusterClient _clusterClient;

        public MatchController(ApplicationDbContext context, MatchRepository matchRepository, IClusterClient clusterClient)
        {
            _context = context;
            _matchRepository = matchRepository;
            _clusterClient = clusterClient;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetMatches()
        {
            var matchListGrain = _clusterClient.GetGrain<IMatchListGrain>(0);
            var matches = await matchListGrain.GetMatches();
            return Ok(matches.Concat(Enumerable.Range(1, 5).Select((x, i) => new Contracts.Models.MatchDto
            {
                Id = Guid.NewGuid().ToString(),
                Name = $"Match {i + 1}",
                CreatedAt = DateTime.UtcNow,
            })));
        }

        [HttpGet("test")]
        public async Task<IActionResult> Get()
        {
            return Ok(new { Value = "abc" });
        }

        [HttpGet("{matchId}")]
        public async Task<IActionResult> GetMatches(string matchId)
        {
            var matchGrain = _clusterClient.GetGrain<IMatchGrain>(matchId);
            var matchInfo = await matchGrain.GetMatchInfo();
            return Ok(matchInfo);
        }

        [HttpPost("new")]
        public async Task<IActionResult> NewMatch([FromBody] NewMatchRequest request)
        {
            var playerGrain = _clusterClient.GetGrain<IPlayerGrain>(request.PlayerId);
            var matchId = await playerGrain.NewMatch(request.MatchName);
            return Ok(new { Id = matchId });
        }

        [HttpPut("{matchId}/invite")]
        [ProducesResponseType(typeof(MatchResponse), 200)]
        public async Task<IActionResult> InvitePlayers(string matchId)
        {
            var match = await _matchRepository.Get(matchId);
            var player = match.Players.Single(x => x.Id == match.PlayerId);
            var playerLevel = player.Level;
            var minLevel = playerLevel >= 10 ? playerLevel - 10 : 1;
            var maxLevel = playerLevel + 10;
            var randomizer = new Random();
            var players = new[]
            {
                PlayerType.Goalkeeper,
                PlayerType.Defender, PlayerType.Defender, PlayerType.Defender, PlayerType.Defender,
                PlayerType.Midfielder, PlayerType.Midfielder, PlayerType.Midfielder, PlayerType.Midfielder,
                PlayerType.Forward, PlayerType.Forward

            }.SelectMany(x => new[] {x, x}).Select(x => (x, randomizer.Next(minLevel, maxLevel))).Select((x, i) =>
                new MatchPlayer
                {
                    Id = player.Id + i + 1,
                    PlayerType = x.Item1,
                    Level = x.Item2,
                    UserName = $"Random {x.Item1.ToString()} Level {x.Item2}"
                });

            match.Players.AddRange(players.Take(21));
            await _matchRepository.Save(match);

            return Ok(new MatchResponse
            {
                MatchId = matchId,
                Players = match.Players.ToArray(),
                Team1 = match.Team1.ToArray(),
                Team2 = match.Team2.ToArray(),
            });
        }

        [HttpPut("{matchId}/join/{playerId}")]
        public async Task<IActionResult> JoinMatch(string matchId, long playerId)
        {
            var playerGrain = _clusterClient.GetGrain<IPlayerGrain>(playerId);
            await playerGrain.JoinMatch(matchId);
            return Ok();
        }

        [HttpPut("{matchId}/captain-pick")]
        public async Task<IActionResult> PickCaptains(string matchId)
        {
            var randomizer = new Random();
            var match = await _matchRepository.Get(matchId);

            var captainTeam1Index = 0;
            match.CaptainTeam1 = match.Players[captainTeam1Index];
            match.Players.RemoveAt(captainTeam1Index);
            
            match.Team1.Add(match.CaptainTeam1);

            var captainTeam2Index = randomizer.Next(match.Players.Count - 1);
            match.CaptainTeam2 = match.Players[captainTeam2Index];
            match.Players.RemoveAt(captainTeam2Index);

            match.Team2.Add(match.CaptainTeam2);

            await _matchRepository.Save(match);

            return Ok(match);
        }

        [HttpPut("{matchId}/player-pick/{playerId}")]
        [ProducesResponseType(typeof(MatchResponse), 200)]
        public async Task<IActionResult> PickPlayer(string matchId, long playerId)
        {
            var randomizer = new Random();
            var match = await _matchRepository.Get(matchId);
            if (match.Players.Count == 0)
            {
                return Ok(match);
            }

            var team1Player = match.Players.Single(x => x.Id == playerId);
            match.Team1.Add(team1Player);
            match.Players.Remove(team1Player);

            var team2PlayerIndex = randomizer.Next(match.Players.Count - 1);
            var team2Player = match.Players[team2PlayerIndex];
            match.Team2.Add(team2Player);
            match.Players.Remove(team2Player);

            return Ok(match);
        }

        [HttpPut("{matchId}/start")]
        [ProducesResponseType(typeof(MatchResponse), 200)]
        public async Task<IActionResult> StartMatch(string matchId)
        {
            var match = await _matchRepository.Get(matchId);

            match.Start().ContinueWith(async t => { await _matchRepository.Save(match); });

            return Ok();
        }

        public class MatchResponse
        {
            public string MatchId { get; set; }

            public MatchPlayer[] Players { get; set; }

            public MatchPlayer[] Team1 { get; set; }

            public MatchPlayer[] Team2 { get; set; }
        }

        public class MatchRepository
        {
            private static readonly Dictionary<string, MatchGrain> _dictionary = new Dictionary<string, MatchGrain>();

            public async Task<MatchGrain> Get(string matchId)
            {
                return _dictionary[matchId];
            }

            public async Task Save(MatchGrain matchGrain)
            {
                _dictionary[matchGrain.MatchId] = matchGrain;
            }
        }

        public class MatchGrain
        {
            public long PlayerId { get; set; }

            public string MatchId { get; set; }

            public MatchPlayer CaptainTeam1 { get; set; }

            public MatchPlayer CaptainTeam2 { get; set; }  

            public List<MatchPlayer> Players { get; set; } = new List<MatchPlayer>();

            public List<MatchPlayer> Team1 { get; set; } = new List<MatchPlayer>();

            public List<MatchPlayer> Team2 { get; set; } = new List<MatchPlayer>();

            public int Minute { get; set; }

            public MatchStatus MatchStatus { get; set; }    

            public async Task Start()
            {
                MatchStatus = MatchStatus.InProgress;

                Console.WriteLine($"Match {MatchId} started");
                
                for (int i = 1; i <= 90; i++)
                {
                    Minute = i;
                    Console.WriteLine(i);
                    await Task.Delay(500);
                }

                MatchStatus = MatchStatus.Finished;

                Console.WriteLine($"Match {MatchId} finished");
            }
        }

        public class MatchEvent
        {
            public int Minute { get; set; }

            public long PlayerId { get; set; }

            public MatchEventType EventType { get; set; }   
        }

        public enum MatchStatus
        {
            Scheduled = 1,
            InProgress,
            Finished
        }

        public enum MatchEventType
        {
            Goal = 1,
            YellowCard,
            RedCard,
            Corner,
            Out,
        }

        public static class MatchExt
        {
            public static IReadOnlyList<MatchEvent> RandomMatchResult()
            {
                var randomizer = new Random();
                var matchEvents = new List<MatchEvent>();

                for (var i = 0; i < 90; i++)
                {

                }

                return matchEvents;
            }
        }

        public class MatchPlayer
        {
            public long? Id { get; set; }
            public string UserName { get; set; }
            public int Level { get; set; }
            public PlayerType PlayerType { get; set; }  
        }

        public class NewMatchRequest
        {
            public long PlayerId { get; set; }

            public string MatchName { get; set; }   

            public PlayerType PlayerType { get; set; }

            public int Level { get; set; }

            public string UserName { get; set; }
        }

        [HttpPost]
        public async Task<IActionResult> ScheduleMatch([FromBody] ScheduleMatchInputModel inputModel)
        {
            var match = new Match
            {
                Name = $"{inputModel.HomeTeamId} vs {inputModel.AwayTeamId}",
                HomeTeamId = inputModel.HomeTeamId,
                AwayTeamId = inputModel.AwayTeamId,
                StartDate = inputModel.StartDate
            };
            await _context.AddAsync(match);
            await _context.SaveChangesAsync();

            return Ok(match.Id);
        }

        [HttpPut("{matchId}")]
        public async Task<IActionResult> StartMatch(int matchId, [FromBody]StartMatchInputModel inputModel)
        {
            var playerHistory = inputModel.HomeTeamPlayers.Concat(inputModel.AwayTeamPlayers).Select(x =>
                new PlayerMatchHistory
                {
                    Id = Guid.NewGuid(),
                    PlayerId = x,
                    MatchId = matchId
                });
            await _context.AddRangeAsync(playerHistory);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}