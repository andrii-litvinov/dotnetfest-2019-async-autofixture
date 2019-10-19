using System.Threading.Tasks;
using Account.Common;
using Contracts.Commands;
using Microsoft.AspNetCore.Mvc;

namespace Account.RequestHandlers
{
    [Route("debit")]
    public class DebitRequestHandler : CommandRequestHandler<DebitAccount>
    {
        public override async Task<IActionResult> Handle(DebitAccount command)
        {
            return Ok(new {command.Amount});
        }
    }
}