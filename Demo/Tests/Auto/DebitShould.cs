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

namespace Tests.Auto
{
    public class DebitShould
    {
        [Theory, Conventions]
        public async Task DecreaseBalance(
            HttpClient client, Account account, IAccountRepository repository)
        {
            // Arrange
            try
            {
                await repository.Create(account);
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
            finally
            {
                await repository.Delete(account.Id);
            }
        }
    }

    public class Conventions : AutoDataAttribute
    {
        public Conventions() : base(Create)
        {
            
        }

        private static IFixture Create()
        {
            var fixture = new Fixture();
            
            var container = new Container();
            var logger = Configuration.CreateLogger();
            var server = new TestServer(Program.ConfigureWebHost(container, logger));
            fixture.Inject(server.CreateClient());
            fixture.Inject(container.GetInstance<IAccountRepository>());
            
            var account = Account.Create(ObjectId.GenerateNewId().ToString());
            account.Credit(50, account.Version);
            fixture.Inject(account);

            return fixture;
        }
    } 
}