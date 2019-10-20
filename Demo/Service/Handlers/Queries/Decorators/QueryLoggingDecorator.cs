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
            var timestamp = Stopwatch.GetTimestamp();
            try
            {
                logger.Debug("Handling {@Query}", query);
                return await decorated.Handle(query);
            }
            finally
            {
                var duration = TimeSpan.FromTicks(Stopwatch.GetTimestamp() - timestamp);
                logger.Debug("{@Query} handled in {@Duration}ms", query, duration.Milliseconds);
            }
        }
    }
}