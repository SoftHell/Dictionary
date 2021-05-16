using System;
using System.Linq;
using System.Threading.Tasks;
using DTO.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace WebApp.ApiControllers.Identity
{
    /// <summary>
    /// Account Authentication
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILogger<AccountController> _logger;
        private readonly IConfiguration _configuration;
        
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="signInManager"></param>
        /// <param name="logger"></param>
        /// <param name="configuration"></param>
        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, ILogger<AccountController> logger, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _configuration = configuration;
        }
        

        /// <summary>
        /// Login to the app
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(JwtResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Message), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<JwtResponse>> Login([FromBody] Login dto)
        {
            var appUser = await _userManager.FindByEmailAsync(dto.Email);
            if (appUser == null)
            {
                _logger.LogWarning("WebApi login. User {User} not found", dto.Email);
                return NotFound(new Message("User/Password problem!"));
            }

            var result = await _signInManager.CheckPasswordSignInAsync(appUser, dto.Password, false);
            if (result.Succeeded)
            {
                var claimsPrincipal = await _signInManager.CreateUserPrincipalAsync(appUser);
                var jwt = Extensions.Base.IdentityExtensions.GenerateJwt(
                    claimsPrincipal.Claims,
                    _configuration["JWT:Key"],
                    _configuration["JWT:Issuer"],
                    _configuration["JWT:Issuer"],
                    DateTime.Now.AddDays(_configuration.GetValue<int>("JWT:ExpireDays"))
                    );
                _logger.LogInformation("WebApi login. User {User}", dto.Email);
                return Ok(new JwtResponse()
                    {
                        Token = jwt,
                        Email = appUser.Email,
                    }
                    );
            }
            _logger.LogWarning("WebApi login. User {User} - password does not match", dto.Email);
            return NotFound(new Message("User/Password problem!"));
        }
        
        /// <summary>
        /// Register as a user
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(JwtResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Message), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<JwtResponse>> Register([FromBody] Register dto)
        {
            var appUser = await _userManager.FindByEmailAsync(dto.Email);
            if (appUser != null)
            {
                _logger.LogWarning(" User {User} already registered", dto.Email);
                return BadRequest(new Message("User already registered"));
            }

            appUser = new IdentityUser()
            {
                Email = dto.Email,
                UserName = dto.Email,
            };
            var result = await _userManager.CreateAsync(appUser, dto.Password);
            
            if (result.Succeeded)
            {
                _logger.LogInformation("User {Email} created a new account with password", appUser.Email);
                
                var user = await _userManager.FindByEmailAsync(appUser.Email);
                if (user != null)
                {                
                    var claimsPrincipal = await _signInManager.CreateUserPrincipalAsync(user);
                    var jwt = Extensions.Base.IdentityExtensions.GenerateJwt(
                        claimsPrincipal.Claims,
                        _configuration["JWT:Key"],                    
                        _configuration["JWT:Issuer"],
                        _configuration["JWT:Issuer"],
                        DateTime.Now.AddDays(_configuration.GetValue<int>("JWT:ExpireDays"))
                    );
                    _logger.LogInformation("WebApi login. User {User}", dto.Email);
                    return Ok(new JwtResponse()
                        {
                            Token = jwt,
                            Email = appUser.Email,
                        }
                    );
                }
                _logger.LogInformation("User {Email} not found after creation", appUser.Email);
                return BadRequest(new Message("User not found after creation!"));
            }
            var errors = result.Errors.Select(error => error.Description).ToList();
            return BadRequest(new Message() {Messages = errors});
        }
    }
}