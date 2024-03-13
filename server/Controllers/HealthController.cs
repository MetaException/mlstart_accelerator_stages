using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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