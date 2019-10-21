using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Tests.Primitives
{
    public abstract class Initializer : IAsyncLifetime
    {
        protected Func<Task> OnDispose { get; set; } = async () => { };
        Task IAsyncLifetime.InitializeAsync() => Initialize();

        async Task IAsyncLifetime.DisposeAsync()
        {
            foreach (var func in OnDispose.GetInvocationList().Cast<Func<Task>>()) await func();
        }

        protected abstract Task Initialize();
    }
}