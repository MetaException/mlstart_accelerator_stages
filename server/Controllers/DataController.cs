using cmd;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace server.Controllers
{
    [Route("api")]
    [ApiController]
    public class DataController : ControllerBase
    {
        [AllowAnonymous]
        [HttpGet("data")]
        public async Task<IActionResult> SendData()
        {
            return Ok(Logger.logEntries);
        }
    }
}
