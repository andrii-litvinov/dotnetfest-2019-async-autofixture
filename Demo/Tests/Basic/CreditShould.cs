//using System.Net;
//using System.Net.Http;
//using System.Threading.Tasks;
//using Contracts.Commands;
//using Domain;
//using FluentAssertions;
//using Microsoft.AspNetCore.TestHost;
//using MongoDB.Bson;
//using Service;
//using Service.Persistence;
//using SimpleInjector;
//using Xunit;
//
//namespace Tests.Basic
//{
//    public class CreditShould : IAsyncLifetime
//    {
//        private readonly HttpClient client;
//        private readonly IAccountRepository repository;
//        private Account account;
//
//        public CreditShould()
//        {
//            var container = new Container();
//            var logger = Configuration.CreateLogger();
//            var server = new TestServer(Program.ConfigureWebHost(container, logger));
//            client = server.CreateClient();
//            repository = container.GetInstance<IAccountRepository>();
//        }
//
//        [Fact]
//        public async Task IncreaseBalanceBySpecifiedAmount()
//        {
//            // Arrange
//            var command = new CreditAccount
//            {
//                Amount = 50,
//                AccountId = account.Id,
//                Version = account.Version
//            };
//
//            // Act
//            var response = await client.PostAsJsonAsync("/credit", command);
//
//            // Assert
//            response.StatusCode.Should().Be(HttpStatusCode.OK);
//            var account1 = await response.Content.ReadAsAsync<Contracts.Queries.Account>();
//            account1.Id.Should().Be(account.Id);
//            account1.Balance.Should().Be(50);
//            account1.Version.Should().Be(2);
//        }
//
//        public async Task InitializeAsync()
//        {
//            account = Account.Create(ObjectId.GenerateNewId().ToString());
//            await repository.Create(account);
//        }
//
//        public async Task DisposeAsync()
//        {
//            await repository.Delete(account.Id);
//        }
//    }
//}