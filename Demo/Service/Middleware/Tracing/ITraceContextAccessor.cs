using Contracts;

namespace Service.Middleware.Tracing
{
    public interface ITraceContextAccessor
    {
        Trace Trace { get; set; }
    }
}