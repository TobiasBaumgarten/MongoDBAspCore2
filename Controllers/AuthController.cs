using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Mongo.Dtos;
using Mongo.Models;

namespace Mongo.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        public AuthController(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody]RegisterDto registerDto)
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
    }
}