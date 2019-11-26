using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Service.Handlers.Requests
{
    [ApiController]
    public abstract class QueryRequestHandler<T> : ControllerBase where T : class
    {
        [HttpGet]
        public abstract Task<IActionResult> Handle([FromUri] T query);
    }

    public class FromUriAttribute : Attribute, IBindingSourceMetadata
    {
        public BindingSource BindingSource { get; } = CompositeBindingSource.Create(
            new[] {BindingSource.Path, BindingSource.Query}, nameof(FromUriAttribute));
    }
}