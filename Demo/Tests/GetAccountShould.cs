using System.Net.Http;
using System.Threading.Tasks;
using Contracts.Queries;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Service;
using SimpleInjector;
using Xunit;

namespace Tests
{
    public class GetAccountShould
    {
        [Fact]
        public async Task ReturnExistingAccount()
        {
            var container = new Container();
            var logger = Configuration.CreateLogger();
            var server = new TestServer(Program.ConfigureWebHost(new WebHostBuilder(), container, logger));
            var client = server.CreateClient();

            var response = await client.GetAsync("get-account/5daafe5ab1ebe053fc76d970");
            var account = await response.Content.ReadAsAsync<Account>();

            account.Id.Should().Be("5daafe5ab1ebe053fc76d970");
            account.Balance.Should().BePositive();
            account.Version.Should().BePositive();
        }
    }
}