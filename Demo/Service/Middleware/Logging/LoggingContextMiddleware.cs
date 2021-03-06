using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Serilog.Context;
using Service.Middleware.Tracing;

namespace Service.Middleware.Logging
{
    public class LoggingContextMiddleware : IMiddleware
    {
        private readonly ITraceContextAccessor accessor;

        public LoggingContextMiddleware(ITraceContextAccessor accessor) => this.accessor = accessor;

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            using var _ = LogContext.PushProperty("CorrelationId", accessor.Trace?.CorrelationId);
            await next(context);
        }
    }
}