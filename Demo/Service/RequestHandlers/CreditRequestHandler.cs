using System.Threading.Tasks;
using Contracts.Commands;
using Microsoft.AspNetCore.Mvc;
using Service.Common;

namespace Service.RequestHandlers
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