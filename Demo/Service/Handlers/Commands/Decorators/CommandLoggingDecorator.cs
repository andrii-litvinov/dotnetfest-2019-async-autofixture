using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Serilog;

namespace Service.Handlers.Commands.Decorators
{
    public class CommandLoggingDecorator<T> : ICommandHandler<T>
    {
        private readonly ICommandHandler<T> decorated;
        private readonly ILogger logger;

        public CommandLoggingDecorator(ICommandHandler<T> decorated, ILogger logger)
        {
            this.decorated = decorated;
            this.logger = logger;
        }

        public async Task Handle(T command)
        {
            logger.Debug("Handling {@Command}", command);

            var timestamp = Stopwatch.GetTimestamp();
            await decorated.Handle(command);

            var duration = TimeSpan.FromTicks(Stopwatch.GetTimestamp() - timestamp);
            logger.Debug("Handled in {@Duration}ms", duration.Milliseconds);
        }
    }
}