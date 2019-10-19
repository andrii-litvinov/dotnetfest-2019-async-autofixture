using System.Threading.Tasks;
using Account.Common;
using Contracts.Commands;
using Microsoft.AspNetCore.Mvc;

namespace Account.RequestHandlers
{
    [Route("create")]
    public class CreateRequestHandler : CommandRequestHandler<CreateAccount>
    {
        public override async Task<IActionResult> Handle(CreateAccount command)
        {
            // TODO: Create and return account.
            return Created(
                Url.Action(
                    "Handle",
                    "GetAccountRequestHandler",
                    new {command.Id}),
                new { });
        }
    }
}