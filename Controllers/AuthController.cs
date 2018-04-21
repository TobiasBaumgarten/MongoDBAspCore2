using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Mongo.Dtos;
using Mongo.Helpers;
using Mongo.Models;

namespace Mongo.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        public AuthController(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterAsync([FromBody]RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var user = new ApplicationUser { UserName = registerDto.UserName, Email = registerDto.Email };
            var result = await userManager.CreateAsync(user, registerDto.Password);

            if (result.Succeeded)
            {
                return Ok("Successfully created");
            }

            return BadRequest(result.Errors);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> LoginAsync([FromBody]LoginDto loginDto, [FromServices] JwtSignInHandler tokenFactory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var user = await userManager.FindByEmailAsync(loginDto.Email);
            if (await userManager.CheckPasswordAsync(user, loginDto.Password))
            {
                string token = CreateToken(user, tokenFactory);
                return Ok(new { token });
            }
            else
            {
                return Unauthorized();
            }
        }

        private string CreateToken(ApplicationUser user, JwtSignInHandler tokenFactory)
        {
            var principal = new ClaimsPrincipal(new[]
            {
                new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.UserName),
                })
            });
            return tokenFactory.BuildJwt(principal);
        }
    }
}