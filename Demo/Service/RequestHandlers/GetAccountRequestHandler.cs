using System.Threading.Tasks;
using Contracts.Queries;
using Microsoft.AspNetCore.Mvc;
using Service.Common;

namespace Service.RequestHandlers
{
    [Route("get-account/{id}")]
    public class GetAccountRequestHandler : QueryRequestHandler<GetAccount>
    {
        public override async Task<IActionResult> Handle(GetAccount query)
        {
            // TODO: Return real account.
            return Ok(new {query.Id});
        }
    }
}