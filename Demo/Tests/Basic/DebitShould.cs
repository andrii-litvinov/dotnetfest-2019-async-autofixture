using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Contracts.Commands;
using Domain;
using FluentAssertions;
using Microsoft.AspNetCore.TestHost;
using MongoDB.Bson;
using Service;
using Service.Persistence;
using SimpleInjector;
using Xunit;

namespace Tests.Basic
{
    public class DebitShould : IAsyncLifetime
    {
        public DebitShould()
        {
            var container = new Container();
            var logger = Configuration.CreateLogger();
            var server = new TestServer(Program.ConfigureWebHost(container, logger));
            client = server.CreateClient();
            repository = container.GetInstance<IAccountRepository>();
        }

        private readonly HttpClient client;
        private readonly IAccountRepository repository;
        private Account account;

        public async Task InitializeAsync()
        {
            account = Account.Create(ObjectId.GenerateNewId().ToString());
            account.Credit(50, account.Version);
            await repository.Create(account);
        }

        public async Task DisposeAsync()
        {
            await repository.Delete(account.Id);
        }

        [Fact]
        public async Task DecreaseBalance()
        {
            // Arrange
            var command = new DebitAccount
            {
                AccountId = account.Id,
                Version = account.Version,
                Amount = -25
            };

            // Act
            var response = await client.PostAsJsonAsync("/debit", command);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var result = await response.Content.ReadAsAsync<Contracts.Queries.Account>();
            result.Balance.Should().Be(25);
        }
    }
}