using AutoFixture;
using SimpleInjector;
using Xunit;

namespace Tests.Primitives
{
    public static class CustomizationExtensions
    {
        public static IFixture Container(this IFixture fixture, Container container)
        {
            container.Options.AllowOverridingRegistrations = true;
            fixture.Inject(container);
            fixture.Customizations.Add(new ContainerSpecimenBuilder(container));
            return fixture;
        }

        public static IFixture InlineData(this IFixture fixture, object[] values)
        {
            fixture.Customizations.Add(new InlineDataSpecimenBuilder(values));
            return fixture;
        }

        public static IFixture Async(this IFixture fixture)
        {
            fixture.Customize(new AsyncCustomization());
            return fixture;
        }

        public static IFixture RegisterInitializer<T>(this IFixture fixture, T instance) where T : IAsyncLifetime
        {
            AsyncContext.Add(instance);
            fixture.Inject(instance);
            return fixture;
        }

        public static IFixture RegisterInitializer<T>(this IFixture fixture) where T : IAsyncLifetime
        {
            fixture.Create<T>();
            return fixture;
        }
    }
}
