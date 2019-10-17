using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Account.RequestHandlers
{
    [ApiController]
    public abstract class RequestHandler<T> : ControllerBase where T : class
    {
        // Handles HTTP-related concerns of request handling.

        [HttpPost]
        public abstract Task<IActionResult> Handle(T message);
    }
}