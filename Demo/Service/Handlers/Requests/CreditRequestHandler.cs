using System.Threading.Tasks;
using Contracts.Commands;
using Contracts.Queries;
using Microsoft.AspNetCore.Mvc;
using Service.Handlers.Commands;
using Service.Handlers.Queries;

namespace Service.Handlers.Requests
{
    [Route("credit")]
    public class CreditRequestHandler : CommandRequestHandler<CreditAccount>
    {
        private readonly ICommandHandler<CreditAccount> handler;
        private readonly IQueryDispatcher dispatcher;

        public CreditRequestHandler(ICommandHandler<CreditAccount> handler, IQueryDispatcher dispatcher)
        {
            this.handler = handler;
            this.dispatcher = dispatcher;
        }

        public override async Task<IActionResult> Handle(CreditAccount command)
        {
            await handler.Handle(command);
            return Ok(await dispatcher.Dispatch(new GetAccount {AccountId = command.AccountId}));
        }
    }
}