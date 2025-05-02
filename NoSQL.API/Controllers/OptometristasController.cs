using Microsoft.AspNetCore.Mvc;

namespace NoSQL.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OptometristasController : ControllerBase
    {
        [HttpGet("test-connection")]
        public IActionResult TestConnection()
        {
            return Ok("Conexión a Couchbase exitosa");
        }
    }
}