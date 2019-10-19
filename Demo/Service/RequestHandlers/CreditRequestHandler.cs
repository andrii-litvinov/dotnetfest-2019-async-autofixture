using System.Threading.Tasks;
using Contracts.Commands;
using Microsoft.AspNetCore.Mvc;
using Service.CommandHandlers;

namespace Service.RequestHandlers
{
    [Route("credit")]
    public class CreditRequestHandler : CommandRequestHandler<CreditAccount>
    {
        private readonly ICommandHandler<CreditAccount> handler;

        public CreditRequestHandler(ICommandHandler<CreditAccount> handler) => this.handler = handler;

        public override async Task<IActionResult> Handle(CreditAccount command)
        {
            await handler.Handle(command);
            return Ok();
        }
    }
}