using System.Threading.Tasks;
using Contracts.Commands;
using Microsoft.AspNetCore.Mvc;
using Service.Common;

namespace Service.RequestHandlers
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