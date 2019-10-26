using AutoFixture;
using Microsoft.AspNetCore.TestHost;
using Service;
using SimpleInjector;

namespace Tests.Async
{
    class TestServerCustomization : ICustomization
    {
        private readonly Container container;
        private TestServer server;

        public TestServerCustomization(Container container)
        {
            this.container = container;
        }

        public void Customize(IFixture fixture)
        {
            var logger = Configuration.CreateLogger();
            server = new TestServer(Program.ConfigureWebHost(container, logger));
            fixture.Inject(server.CreateClient());
        }
    }
}