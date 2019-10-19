using System.Threading;

namespace Service.Tracing
{
    public class TraceContextAccessor : ITraceContextAccessor
    {
        // TODO: Set in middleware.

        private static readonly AsyncLocal<Trace> trace = new AsyncLocal<Trace>();

        public Trace Trace
        {
            get => trace.Value;
            set => trace.Value = value;
        }
    }
}