using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Orleans;
using _101mngr.Contracts;
using Microsoft.AspNetCore.Authorization;
using _101mngr.Contracts.Enums;
using _101mngr.WebApp.Hubs;

namespace _101mngr.WebApp.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class MatchController : Controller    
    {
        private readonly IClusterClient _clusterClient;
        private readonly MatchRoomService _matchRoomService;

        public MatchController(IClusterClient clusterClient, MatchRoomService matchRoomService)
        {
            _clusterClient = clusterClient;
            _matchRoomService = matchRoomService;
        }

        [AllowAnonymous]
        [HttpGet("")]
        public async Task<IActionResult> GetCurrentMatches(bool finished)
        {
            var matchListGrain = _clusterClient.GetGrain<IMatchRegistryGrain>(0);
            var matches = !finished
                ? await matchListGrain.GetCurrentMatches()
                : await matchListGrain.GetFinishedMatches();
            return Ok(matches);
        }

        [HttpPost("new")]
        public async Task<IActionResult> NewMatch([FromBody] NewMatchRequest request)
        {
            var accountId = long.Parse(HttpContext.User.Claims.FirstOrDefault(x => x.Type == "sub")?.Value);
            var matchId = Guid.NewGuid().ToString();
            var playerGrain = _clusterClient.GetGrain<IPlayerGrain>(accountId);
            var playerInfo = await playerGrain.GetPlayerInfo();
            await _matchRoomService.NewMatchRoom(
                matchId, accountId, GetFullName(playerInfo.FirstName, playerInfo.LastName));
            return Ok(new { Id = matchId });
        }

        [HttpPut("{matchId}/join")]
        public async Task<IActionResult> JoinMatch(string matchId)
        {
            long playerId = this.GetSubjectId();
            var playerGrain = _clusterClient.GetGrain<IPlayerGrain>(playerId);
            var playerInfo = await playerGrain.GetPlayerInfo();
            await _matchRoomService.JoinRoom(matchId, playerId, GetFullName(playerInfo.FirstName, playerInfo.LastName));
            return Ok();
        }

        [HttpPut("{matchId}/leave")]
        public async Task<IActionResult> LeaveMatch(string matchId)
        {
            long playerId = this.GetSubjectId();
            await _matchRoomService.LeaveRoom(matchId, playerId);
            return Ok();
        }

        [HttpPut("{matchId}/start")]
        public async Task<IActionResult> StartMatch(string matchId)
        {
            await _matchRoomService.StartMatch(matchId);
            return Ok();
        }

        private static string GetFullName(string firstName, string lastName)
        {
            return $"{firstName} {lastName}";
        }

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