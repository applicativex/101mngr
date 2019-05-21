using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Orleans;
using _101mngr.Contracts;

namespace _101mngr.WebApp.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class TrainingController : Controller
    {
        private readonly IClusterClient _clusterClient;

        public TrainingController(IClusterClient clusterClient)
        {
            _clusterClient = clusterClient;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetTraining()
        {
            var accountId = long.Parse(HttpContext.User.Claims.FirstOrDefault(x => x.Type == "sub")?.Value);
            var playerGrain = _clusterClient.GetGrain<IPlayerGrain>(accountId);
            var training = await playerGrain.GetCurrentTraining();
            return Ok(training);
        }

        [HttpPut("start")]
        public async Task<IActionResult> StartTraining()
        {
            var accountId = long.Parse(HttpContext.User.Claims.FirstOrDefault(x => x.Type == "sub")?.Value);
            var playerGrain = _clusterClient.GetGrain<IPlayerGrain>(accountId);
            await playerGrain.StartTraining();
            return Ok();
        }

        [HttpPut("passing")]
        public async Task<IActionResult> Passing()
        {
            var accountId = long.Parse(HttpContext.User.Claims.FirstOrDefault(x => x.Type == "sub")?.Value);
            var playerGrain = _clusterClient.GetGrain<IPlayerGrain>(accountId);
            var result = await playerGrain.TrainPassing();
            return Ok(result);
        }

        [HttpPut("endurance")]
        public async Task<IActionResult> Endurance()
        {
            var accountId = long.Parse(HttpContext.User.Claims.FirstOrDefault(x => x.Type == "sub")?.Value);
            var playerGrain = _clusterClient.GetGrain<IPlayerGrain>(accountId);
            var result = await playerGrain.TrainEndurance();
            return Ok(result);
        }

        [HttpPut("dribbling")]
        public async Task<IActionResult> Dribbling()
        {
            var accountId = long.Parse(HttpContext.User.Claims.FirstOrDefault(x => x.Type == "sub")?.Value);
            var playerGrain = _clusterClient.GetGrain<IPlayerGrain>(accountId);
            var result = await playerGrain.TrainDribbling();
            return Ok(result);
        }

        [HttpPut("coverage")]
        public async Task<IActionResult> Coverage()
        {
            var accountId = long.Parse(HttpContext.User.Claims.FirstOrDefault(x => x.Type == "sub")?.Value);
            var playerGrain = _clusterClient.GetGrain<IPlayerGrain>(accountId);
            var result = await playerGrain.TrainCoverage();
            return Ok(result);
        }

        [HttpPut("finish")]
        public async Task<IActionResult> FinishTraining()
        {
            var accountId = long.Parse(HttpContext.User.Claims.FirstOrDefault(x => x.Type == "sub")?.Value);
            var playerGrain = _clusterClient.GetGrain<IPlayerGrain>(accountId);
            await playerGrain.FinishTraining();
            return Ok();
        }
    }
}