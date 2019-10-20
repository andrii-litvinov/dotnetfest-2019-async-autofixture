using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Service.Handlers.Requests
{
    [ApiController]
    public abstract class CommandRequestHandler<T> : ControllerBase where T : class
    {
        [HttpPost]
        public abstract Task<IActionResult> Handle(T command);
    }
}