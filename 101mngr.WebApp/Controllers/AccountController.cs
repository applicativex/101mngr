using System;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Orleans;
using _101mngr.AuthorizationServer.Models;
using _101mngr.Contracts;
using _101mngr.Contracts.Models;
using _101mngr.WebApp.Models.Requests;
using _101mngr.WebApp.Services;

namespace _101mngr.WebApp.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly AuthorizationService _authorizationService;
        private readonly IClusterClient _clusterClient;

        public AccountController(AuthorizationService authorizationService, IClusterClient clusterClient)
        {
            _authorizationService = authorizationService;
            _clusterClient = clusterClient;
        }

        [AllowAnonymous]
        [HttpGet("{accountId}")]
        public async Task<IActionResult> GetId(long accountId)
        {
            var playerGrain = _clusterClient.GetGrain<IPlayerGrain>(accountId);
            var result = await playerGrain.GetPlayer();
            return Ok(result);
        }

        /// <summary>
        /// Register new user
        /// </summary>
        [ProducesResponseType(typeof(long), (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterInputModel inputModel)
        {
            if (!IsCountryCodeValid(inputModel.CountryCode))
            {
                return BadRequest("Invalid country code");
            }

            var accountId = await _authorizationService.Register(
                inputModel.UserName, inputModel.Email, inputModel.Password, inputModel.CountryCode);
            var playerGrain = _clusterClient.GetGrain<IPlayerGrain>(accountId);
            await playerGrain.Create(new CreatePlayerDto
            {
                UserName = inputModel.UserName,
                CountryCode = inputModel.CountryCode,
                Email = inputModel.Email
            });
            return Ok(new { Id = accountId });

            bool IsCountryCodeValid(string countryCode) => CultureInfo
                .GetCultures(CultureTypes.SpecificCultures)
                .Select(culture => new RegionInfo(culture.Name))
                .Any(region => region.TwoLetterISORegionName == countryCode);
        }

        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginInputModel model)
        {
            var token = await _authorizationService.Login(model.Email, model.Password);
            return Ok(new { Token = $"Bearer {token}" });
        }

        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            var accountId = long.Parse(HttpContext.User.Claims.FirstOrDefault(x => x.Type == "sub")?.Value);
            var userInfo = await _authorizationService.GetUserInfo(accountId);
            return Content(userInfo, "application/json");
        }

        [AllowAnonymous]
        [HttpPost("username/available")]
        public async Task<IActionResult> CheckUserName()
        {
            return Ok(new { IsAvailable = true});
        }

        /// <summary>
        /// Logouts user
        /// </summary>
        [HttpPost("logout")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> Logout()
        {
            try
            {
                var token = await HttpContext.GetTokenAsync("access_token");
                await _authorizationService.Logout(token);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
    }
}