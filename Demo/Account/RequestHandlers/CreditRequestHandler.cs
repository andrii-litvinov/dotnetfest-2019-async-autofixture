using System.Threading.Tasks;
using Account.Common;
using Contracts.Commands;
using Microsoft.AspNetCore.Mvc;

namespace Account.RequestHandlers
{
    [Route("credit")]
    public class CreditRequestHandler : CommandRequestHandler<CreditAccount>
    {
        public override async Task<IActionResult> Handle(CreditAccount command)
        {
            return Ok();
        }
    }
}