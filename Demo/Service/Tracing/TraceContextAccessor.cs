using System.Threading;
using Contracts;

namespace Service.Tracing
{
    public class TraceContextAccessor : ITraceContextAccessor
    {
        // TODO: Investigate how it correlates with Activity and distributed tracing.

        private static readonly AsyncLocal<Trace> trace = new AsyncLocal<Trace>();

        public Trace Trace
        {
            get => trace.Value;
            set => trace.Value = value;
        }
    }
}