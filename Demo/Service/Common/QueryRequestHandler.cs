using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Service.Common
{
    [ApiController]
    public abstract class QueryRequestHandler<T> : ControllerBase where T : class
    {
        [HttpGet]
        public abstract Task<IActionResult> Handle(T message);
    }
}