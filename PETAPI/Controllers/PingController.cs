using Microsoft.AspNetCore.Mvc;

namespace PETAPI.Controllers
{
    [ApiController]
    [Route("ping")]
    public class PingController : ControllerBase
    {
        [HttpGet]
        public string Ping()
        {
            return "Dogshouseservice.Version1.0.1";
        }
    }
}
