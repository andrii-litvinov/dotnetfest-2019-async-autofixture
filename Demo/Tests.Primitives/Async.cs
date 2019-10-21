using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.Kernel;
using Xunit;

namespace Tests.Primitives
{
    public abstract class Async : IAsyncLifetime
    {
        protected Async()
        {
            foreach (var asyncLifetime in AsyncContext.GetAsyncLifetimes())
            {
                OnInit += asyncLifetime.InitializeAsync;
                OnDispose += asyncLifetime.DisposeAsync;
            }
        }

        private Func<Task> OnDispose { get; } = async () => { };
        private Func<Task> OnInit { get; } = async () => { };

        async Task IAsyncLifetime.InitializeAsync() => await OnInit.InvokeSequentially();

        async Task IAsyncLifetime.DisposeAsync() => await OnDispose.InvokeSequentially();
    }

    internal static class InvocationExtensions
    {
        public static async Task InvokeSequentially(this Func<Task> func)
        {
            foreach (var f in func.GetInvocationList().Cast<Func<Task>>()) await f();
        }
    }

    internal class AsyncContext
    {
        private static readonly ThreadLocal<HashSet<IAsyncLifetime>> lifetimes =
            new ThreadLocal<HashSet<IAsyncLifetime>>();

        public static void Init() => lifetimes.Value = new HashSet<IAsyncLifetime>();
        public static void Add(IAsyncLifetime lifetime) => lifetimes.Value.Add(lifetime);
        public static IEnumerable<IAsyncLifetime> GetAsyncLifetimes() =>
            lifetimes?.Value ?? Enumerable.Empty<IAsyncLifetime>();
    }

    internal class AsyncCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            AsyncContext.Init();
            fixture.Behaviors.Add(new AsyncTransformation());
        }
    }

    internal class AsyncTransformation : ISpecimenBuilderTransformation
    {
        public ISpecimenBuilderNode Transform(ISpecimenBuilder builder) =>
            new AsyncBuilderNode(builder);
    }

    internal class AsyncBuilderNode : ISpecimenBuilderNode
    {
        private readonly ISpecimenBuilder builder;

        public AsyncBuilderNode(ISpecimenBuilder builder) => this.builder = builder;

        public object Create(object request, ISpecimenContext context)
        {
            var specimen = builder.Create(request, context);
            if (specimen is IAsyncLifetime asyncSpecimen) AsyncContext.Add(asyncSpecimen);
            return specimen;
        }

        public IEnumerator<ISpecimenBuilder> GetEnumerator()
        {
            yield return builder;
        }

        public ISpecimenBuilderNode Compose(IEnumerable<ISpecimenBuilder> builders) =>
            new AsyncBuilderNode(new CompositeSpecimenBuilder(builders));

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
