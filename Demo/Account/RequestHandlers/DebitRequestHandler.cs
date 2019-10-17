using System.Threading.Tasks;
using Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Account.RequestHandlers
{
    [Route("debit")]
    public class DebitRequestHandler : RequestHandler<DebitAccount>
    {
        public override async Task<IActionResult> Handle(DebitAccount command)
        {
            return Ok();
        }
    }
}