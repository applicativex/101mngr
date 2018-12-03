using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using _101mngr.AuthorizationServer.Data;
using _101mngr.AuthorizationServer.Models;

namespace _101mngr.AuthorizationServer.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly ILogger<AccountController> _logger;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
            ApplicationDbContext applicationDbContext, ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _applicationDbContext = applicationDbContext;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterInputModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = new ApplicationUser
                    {
                        UserName = model.UserName,
                        Email = model.Email,
                        CountryCode = model.CountryCode,
                        BirthDate = DateTime.Today
                    };

                    var result = await _userManager.CreateAsync(user, model.Password);

                    if (result.Succeeded)
                    {
                        return Ok(user.Id);
                    }

                    var emailError = result.Errors.FirstOrDefault(error => error.Code == "DuplicateEmail");
                    if (emailError != null)
                    {
                        ModelState.AddModelError("Email", emailError.Description);
                    }
                    else
                    {
                        throw new Exception(string.Join(",", result.Errors.Select(e => $"{e.Code}:{e.Description}")));
                    }
                }

                return BadRequest(ModelState);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error register new user.");
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }
        
        [HttpGet("username/{email}")]
        public async Task<IActionResult> GetUserName(string email)
        {
            var user = await _applicationDbContext.Users.FirstOrDefaultAsync(x => x.Email == email);
            return Ok(user.UserName);
        }

        [HttpGet("{userId}/userinfo")]
        public async Task<object> GetUserInfo(long userId)
        {
            var user = await _applicationDbContext.Users.FirstOrDefaultAsync(x => x.Id == userId);
            return Ok(new
            {
                user.Id,
                user.UserName,
                user.Email,
                user.CountryCode,
                user.BirthDate
            });
        }
    }
}