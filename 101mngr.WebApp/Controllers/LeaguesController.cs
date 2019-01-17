using System.Linq;
using Microsoft.AspNetCore.Mvc;
using _101mngr.WebApp.Domain;

namespace _101mngr.WebApp.Controllers
{
    [Route("api/[controller]")]
    public class LeaguesController : Controller
    {
        private readonly FootballLeagueService _leagueService;

        public LeaguesController(FootballLeagueService leagueService)
        {
            _leagueService = leagueService;
        }

        [HttpGet("countries")]
        public IActionResult GetCountries()
        {
            var countries = _leagueService.GetCountries();
            return Ok(countries);
        }

        [HttpGet("")]
        public IActionResult GetLeagues([FromQuery]string countryId)
        {
            var leagues = _leagueService.GetLeagues(countryId);
            return Ok(leagues);
        }

        [HttpGet("{leagueId}")]
        public IActionResult GetLeague(string leagueId)
        {
            var league = _leagueService.GetLeague(leagueId);
            return Ok(league);
        }

        [HttpGet("{leagueId}/seasons")]
        public IActionResult GetLeagueSeasons(string leagueId)
        {
            var seasons = _leagueService.GetLeagueSeasons(leagueId);
            return Ok(seasons);
        }

        [HttpGet("{leagueId}/seasons/{seasonId}/teams")]
        public IActionResult GetTeams(string leagueId, string seasonId)
        {
            var teams = _leagueService.GetSeasonTeams(seasonId);
            return Ok(teams);
        }

        [HttpGet("{leagueId}/seasons/{seasonId}/matches")] 
        public IActionResult GetMatches()
        {
            return Ok();
        }

        [HttpGet("{leagueId}/seasons/{seasonId}/results")]
        public IActionResult GetResults()
        {
            return Ok();
        }
        // leagues/2323/seasons/938e9/matches
        [HttpGet("{leagueId}/seasons/{seasonId}/table")]
        public IActionResult GetTable()
        {
            return Ok();
        }
    }
}