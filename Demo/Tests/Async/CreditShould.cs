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
using Tests.Primitives;
using Xunit;

namespace Tests.Async
{
    public class CreditShould : Primitives.Async
    {
        [Theory, Conventions]
        public async Task IncreaseBalanceBySpecifiedAmount(Account account, HttpClient client)
        {
            // Arrange
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
            account1.Balance.Should().Be(50);
            account1.Version.Should().Be(2);
        }
    }

    public class Conventions : AutoDataAttribute
    {
        public Conventions() : base(Configure)
        {
        }

        private static IFixture Configure()
        {
            var container = new Container();
            var fixture = new Fixture()
                .Container(container)
                .Async()
                .Customize(new TestClientCustomization(container))
                .Customize(new AccountCustomization());
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

    public class AccountCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            var account = Account.Create(ObjectId.GenerateNewId().ToString());
            fixture.Inject(account);
            fixture.RegisterInitializer<AccountInitializer>();
        }
    }

    public class AccountInitializer : Initializer
    {
        private readonly Account account;
        private readonly IAccountRepository repository;

        public AccountInitializer(Account account, IAccountRepository repository)
        {
            this.account = account;
            this.repository = repository;
        }

        protected override async Task Initialize()
        {
            await repository.Create(account);
            OnDispose += () => repository.Delete(account.Id);
        }
    }
}