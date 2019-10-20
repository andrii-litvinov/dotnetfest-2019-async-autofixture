using System;
using System.Threading.Tasks;
using Contracts;
using Microsoft.AspNetCore.Http;

namespace Service.Tracing
{
    public class TracingMiddleware : IMiddleware
    {
        private readonly ITraceContextAccessor accessor;

        public TracingMiddleware(ITraceContextAccessor accessor) => this.accessor = accessor;

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var correlationId = context.Request.Headers.TryGetValue("X-Correlation-ID", out var value)
                ? (string) value
                : Guid.NewGuid().ToString();

            accessor.Trace = new Trace {CorrelationId = correlationId};
            await next(context);
        }
    }
}