using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using _101mngr.AuthorizationServer.Models;
using _101mngr.WebApp.Data;

namespace _101mngr.WebApp.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class PlayerController : Controller
    {
        private readonly IPlayerRepository _playerRepository;

        public PlayerController(IPlayerRepository playerRepository)
        {
            _playerRepository = playerRepository;
        }

        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [HttpPost("")]
        public async Task<IActionResult> AddPlayer([FromBody] AddPlayerInputModel model)
        {
            await _playerRepository.AddPlayer(new Player
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                CountryCode = model.CountryCode,
                BirthDate = model.BirthDate,
                Aggression = model.Aggression,
                Height = model.Height,
                Weight = model.Weight,
                AccountId = this.GetSubjectId()
            });
            return Ok();
        }
    }
}