using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using server.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace server.Controllers
{
    [Route("api")]
    [ApiController]
    public class HealthController : ControllerBase
    {
        [AllowAnonymous]
        [HttpGet("health")]
        public async Task<IActionResult> CheckHealth()
        {
            return Ok("Server is working correctly");
        }
    }
}
