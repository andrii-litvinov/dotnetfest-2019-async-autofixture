using System.Threading.Tasks;
using Contracts.Commands;
using Contracts.Queries;
using Microsoft.AspNetCore.Mvc;
using Service.CommandHandlers;
using Service.QueryHandlers;

namespace Service.RequestHandlers
{
    [Route("debit")]
    public class DebitRequestHandler : CommandRequestHandler<DebitAccount>
    {
        private readonly ICommandHandler<DebitAccount> handler;
        private readonly IQueryDispatcher dispatcher;

        public DebitRequestHandler(ICommandHandler<DebitAccount> handler, IQueryDispatcher dispatcher)
        {
            this.handler = handler;
            this.dispatcher = dispatcher;
        }

        public override async Task<IActionResult> Handle(DebitAccount command)
        {
            await handler.Handle(command);
            return Ok(await dispatcher.Dispatch(new GetAccount {AccountId = command.AccountId}));
        }
    }
}