namespace Service.Tracing
{
    public interface ITraceContextAccessor
    {
        Trace Trace { get; set; }
    }
}