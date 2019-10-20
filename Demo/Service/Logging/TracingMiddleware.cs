using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Serilog.Context;
using Service.Tracing;

namespace Service.Logging
{
    public class LoggingMiddleware : IMiddleware
    {
        private readonly ITraceContextAccessor accessor;

        public LoggingMiddleware(ITraceContextAccessor accessor) => this.accessor = accessor;

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            using var _ = LogContext.PushProperty("CorrelationId", accessor.Trace?.CorrelationId);
            await next(context);
        }
    }
}