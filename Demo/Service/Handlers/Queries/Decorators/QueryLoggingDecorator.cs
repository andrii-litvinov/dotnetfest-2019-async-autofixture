using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Contracts.Queries;
using Serilog;

namespace Service.Handlers.Queries.Decorators
{
    public class QueryLoggingDecorator<TQuery, TResult> : IQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult>
    {
        private readonly IQueryHandler<TQuery, TResult> decorated;
        private readonly ILogger logger;

        public QueryLoggingDecorator(IQueryHandler<TQuery, TResult> decorated, ILogger logger)
        {
            this.decorated = decorated;
            this.logger = logger;
        }

        public async Task<TResult> Handle(TQuery query)
        {
            logger.Debug("Handling {@Query}", query);

            var timestamp = Stopwatch.GetTimestamp();
            var result = await decorated.Handle(query);

            var duration = TimeSpan.FromTicks(Stopwatch.GetTimestamp() - timestamp);
            logger.Debug("Handled in {@Duration}ms", duration.Milliseconds);

            return result;
        }
    }
}