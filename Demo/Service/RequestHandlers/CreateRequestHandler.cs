using System.Threading.Tasks;
using Contracts.Commands;
using Contracts.Queries;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Service.CommandHandlers;
using Service.QueryHandlers;

namespace Service.RequestHandlers
{
    [Route("create")]
    public class CreateRequestHandler : CommandRequestHandler<CreateAccount>
    {
        private readonly ICommandHandler<CreateAccount> handler;
        private readonly IQueryDispatcher dispatcher;

        public CreateRequestHandler(ICommandHandler<CreateAccount> handler, IQueryDispatcher dispatcher)
        {
            this.handler = handler;
            this.dispatcher = dispatcher;
        }

        public override async Task<IActionResult> Handle(CreateAccount command)
        {
            command.AccountId = ObjectId.GenerateNewId().ToString();
            await handler.Handle(command);
            return Created(
                Url.Action("Handle", "GetAccountRequestHandler", new {command.AccountId}),
                await dispatcher.Dispatch(new GetAccount {AccountId = command.AccountId}));
        }
    }
}