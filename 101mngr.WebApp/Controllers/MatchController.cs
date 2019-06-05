using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Orleans;
using _101mngr.Contracts;
using Microsoft.AspNetCore.Authorization;
using Orleans.Streams;
using _101mngr.Contracts.Enums;
using _101mngr.Contracts.Models;

namespace _101mngr.WebApp.Controllers
{
    public class Obs : IAsyncObserver<MatchListItemDto>
    {
        public Task OnNextAsync(MatchListItemDto item, StreamSequenceToken token = null)
        {
            Console.WriteLine($"Stream update {item.Id} {item.Name} {item.PlayersCount}");
            return Task.CompletedTask;
        }

        public Task OnCompletedAsync()
        {
            Console.WriteLine("Completed");
            return Task.CompletedTask;
        }

        public Task OnErrorAsync(Exception ex)
        {
            Console.WriteLine(ex);
            return Task.CompletedTask;
        }
    }
    [Authorize]
    [Route("api/[controller]")]
    public class MatchController : Controller    
    {
        private readonly IClusterClient _clusterClient;

        public MatchController(IClusterClient clusterClient)
        {
            _clusterClient = clusterClient;
        }

        [AllowAnonymous]
        [HttpGet("test")]
        public async Task<IActionResult> Test()
        {
            var streamProvider = _clusterClient.GetStreamProvider("SMSProvider");
            var playerGrain = _clusterClient.GetGrain<IPlayerGrain>(1);
            var matchId = await playerGrain.NewMatch("sdfsf");
            var stream = streamProvider.GetStream<MatchListItemDto>(Guid.Parse(matchId), "Matches");
            await stream.SubscribeAsync(new Obs());
            for (int i = 0; i < 1000; i++)
            {
                await _clusterClient.GetGrain<IMatchGrain>(matchId).JoinMatch(i + 1, $"lol {i + 1}", false);
                await Task.Delay(100);
            }
            await Task.Delay(10000);
            return Ok();
        }

        [AllowAnonymous]
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

        [AllowAnonymous]
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
            var accountId = long.Parse(HttpContext.User.Claims.FirstOrDefault(x => x.Type == "sub")?.Value);
            var playerGrain = _clusterClient.GetGrain<IPlayerGrain>(accountId);
            var matchId = await playerGrain.NewMatch(request.MatchName);
            return Ok(new { Id = matchId });
        }

        [HttpPost("random")]
        public async Task<IActionResult> RandomMatch()
        {
            var accountId = long.Parse(HttpContext.User.Claims.FirstOrDefault(x => x.Type == "sub")?.Value);
            var playerGrain = _clusterClient.GetGrain<IPlayerGrain>(accountId);
            var matchId = await playerGrain.RandomMatch();
            return Ok(new { Id = matchId });
        }

        [HttpPut("{matchId}/join")]
        public async Task<IActionResult> JoinMatch(string matchId)
        {
            long playerId = this.GetSubjectId();
            var playerGrain = _clusterClient.GetGrain<IPlayerGrain>(playerId);
            await playerGrain.JoinMatch(matchId);
            return Ok();
        }

        [HttpPut("{matchId}/leave")]
        public async Task<IActionResult> LeaveMatch(string matchId)
        {
            long playerId = this.GetSubjectId();
            var playerGrain = _clusterClient.GetGrain<IPlayerGrain>(playerId);
            await playerGrain.LeaveMatch(matchId);
            return Ok();
        }

        [HttpPut("{matchId}/start")]
        public async Task<IActionResult> StartMatch(string matchId)
        {
            var matchGrain = _clusterClient.GetGrain<IMatchGrain>(matchId);
            await matchGrain.PlayMatch();
            return Ok();
        }

        //[AllowAnonymous]
        //[HttpGet("test")]
        //public async Task<IActionResult> Get()
        //{
        //    return Ok(new { Value = "abc" });
        //}
        
        public class NewMatchRequest
        {
            public long PlayerId { get; set; }

            public string MatchName { get; set; }   

            public PlayerType PlayerType { get; set; }

            public int Level { get; set; }

            public string UserName { get; set; }
        }
    }
}