using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using server.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

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
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, true, false);

            if (result.Succeeded)
            {
                // Пользователь успешно аутентифицирован
                return Ok(new { Token = GenerateToken(model.UserName) });
            }
            else
            {
                // Неудачная аутентификация
                return Unauthorized();
            }
        }

        private string GenerateToken(string username)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("your-secret-key"); //var key = Environment.GetEnvironmentVariable("YOUR_SECRET_KEY");

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
        public async Task<IActionResult> Register([FromBody] LoginModel model)
        {
            var user = new LoginModel { UserName = model.UserName, Password = model.Password };

            _userManager.UserValidators.Clear();
            var result = await _userManager.CreateAsync(user);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                return Ok(new { Token = GenerateToken(model.UserName) });
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }

    }
}
