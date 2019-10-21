using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.Xunit2;
using Contracts.Commands;
using Domain;
using FluentAssertions;
using Microsoft.AspNetCore.TestHost;
using MongoDB.Bson;
using Service;
using Service.Persistence;
using SimpleInjector;
using Xunit;

namespace Tests
{
    public class CreditShould2
    {
        [Theory, AccountWithBalanceConventions]
        public async Task IncreaseBalanceBySpecifiedAmount(
            Account account, HttpClient client, IAccountRepository repository)
        {
            // Arrange
            try
            {
                await repository.Create(account);

                var command = new CreditAccount
                {
                    Amount = 50,
                    AccountId = account.Id,
                    Version = account.Version
                };

                // Act
                var response = await client.PostAsJsonAsync("/credit", command);

                // Assert
                response.StatusCode.Should().Be(HttpStatusCode.OK);
                var account1 = await response.Content.ReadAsAsync<Contracts.Queries.Account>();
                account1.Id.Should().Be(account.Id);
                account1.Balance.Should().Be(200);
                account1.Version.Should().Be(3);
            }
            finally
            {
                await repository.Delete(account.Id);
            }
        }
    }

    public class AccountWithBalanceConventions : AutoDataAttribute
    {
        public AccountWithBalanceConventions() : base(Configure)
        {
        }

        private static IFixture Configure()
        {
            var container = new Container();
            var fixture = new Fixture()
                .Customize(new TestClientCustomization(container))
                .Customize(new AccountWithBalanceCustomization());
            fixture.Inject(container.GetInstance<IAccountRepository>());
            return fixture;
        }
    }

    public class TestClientCustomization : ICustomization
    {
        private readonly Container container;

        public TestClientCustomization(Container container) => this.container = container;

        public void Customize(IFixture fixture)
        {
            var logger = Configuration.CreateLogger();
            var server = new TestServer(Program.ConfigureWebHost(container, logger));
            fixture.Inject(server.CreateClient());
        }
    }

    public class AccountWithBalanceCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            var account = Account.Create(ObjectId.GenerateNewId().ToString());
            account.Credit(150, account.Version);
            fixture.Inject(account);
        }
    }
}