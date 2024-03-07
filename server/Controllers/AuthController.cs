using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using server.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly SignInManager<LoginModel> _signInManager;
        private readonly UserManager<LoginModel> _userManager;

        public AuthController(SignInManager<LoginModel> signInManager, UserManager<LoginModel> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserModel model)
        {
            var user = await _userManager.FindByNameAsync(model.Login);

            if (user is null)
            {
                return Unauthorized();
            }
            
            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                return Ok(new { Token = GenerateToken(user.UserName) });
            }
            else
            {
                return Unauthorized();
            }
        }

        private string GenerateToken(string username)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("CCEA5D06F64497D9CCB548B70B024ASGESGHES5H50"); 

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, username)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }


        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserModel model)
        {
            var localUser = new LoginModel { UserName = model.Login };

            if (await _userManager.FindByNameAsync(localUser.UserName) is not null)
            {
                return Conflict("Account is already exists");
            }

            var result = await _userManager.CreateAsync(localUser, model.Password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(localUser, isPersistent: false);    
                return Ok(new { Token = GenerateToken(localUser.UserName) });
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }

    }
}
