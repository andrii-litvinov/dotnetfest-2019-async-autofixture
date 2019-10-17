using System.Threading.Tasks;
using Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Account.RequestHandlers
{
    [Route("credit")]
    public class CreditRequestHandler : RequestHandler<CreditAccount>
    {
        public override async Task<IActionResult> Handle(CreditAccount command)
        {
            return Ok();
        }
    }
}