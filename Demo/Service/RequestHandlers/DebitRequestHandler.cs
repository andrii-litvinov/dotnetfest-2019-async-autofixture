using System.Threading.Tasks;
using Contracts.Commands;
using Microsoft.AspNetCore.Mvc;
using Service.Common;

namespace Service.RequestHandlers
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