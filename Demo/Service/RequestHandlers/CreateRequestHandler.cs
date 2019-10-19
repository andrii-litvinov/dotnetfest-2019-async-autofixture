using System.Threading.Tasks;
using Contracts.Commands;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Service.CommandHandlers;

namespace Service.RequestHandlers
{
    [Route("create")]
    public class CreateRequestHandler : CommandRequestHandler<CreateAccount>
    {
        private readonly ICommandHandler<CreateAccount> handler;

        public CreateRequestHandler(ICommandHandler<CreateAccount> handler) => this.handler = handler;

        public override async Task<IActionResult> Handle(CreateAccount command)
        {
            command.Id = ObjectId.GenerateNewId().ToString();
            await handler.Handle(command);
            return Created(Url.Action("Handle","GetAccountRequestHandler",new {command.Id}),null);
        }
    }
}