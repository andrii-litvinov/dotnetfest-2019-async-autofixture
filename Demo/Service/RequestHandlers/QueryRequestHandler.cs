using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Service.RequestHandlers
{
    [ApiController]
    public abstract class QueryRequestHandler<T> : ControllerBase where T : class
    {
        [HttpGet]
        public abstract Task<IActionResult> Handle([FromRoute, FromQuery] T message);
    }
}