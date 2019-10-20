using System.Threading.Tasks;
using Contracts.Queries;
using Microsoft.AspNetCore.Mvc;
using Service.Handlers.Queries;

namespace Service.Handlers.Requests
{
    [Route("get-account/{accountId}")]
    public class GetAccountRequestHandler : QueryRequestHandler<GetAccount>
    {
        private readonly IQueryDispatcher dispatcher;

        public GetAccountRequestHandler(IQueryDispatcher dispatcher) => this.dispatcher = dispatcher;

        public override async Task<IActionResult> Handle(GetAccount query)
        {
            var account = await dispatcher.Dispatch(query);
            return account != null ? (IActionResult) Ok(account) : NotFound();
        }
    }
}