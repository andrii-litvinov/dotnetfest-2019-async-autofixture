using System.Threading.Tasks;
using Contracts.Commands;
using Microsoft.AspNetCore.Mvc;
using Service.CommandHandlers;

namespace Service.RequestHandlers
{
    [Route("debit")]
    public class DebitRequestHandler : CommandRequestHandler<DebitAccount>
    {
        private readonly ICommandHandler<DebitAccount> handler;

        public DebitRequestHandler(ICommandHandler<DebitAccount> handler) => this.handler = handler;

        public override async Task<IActionResult> Handle(DebitAccount command)
        {
            await handler.Handle(command);
            return Ok();
        }
    }
}