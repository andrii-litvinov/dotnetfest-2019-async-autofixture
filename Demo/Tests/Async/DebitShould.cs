using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Contracts.Commands;
using Domain;
using FluentAssertions;
using Xunit;

namespace Tests.Async
{
    public class DebitShould : Primitives.Async
    {
        [Theory, AccountWithBalanceConventions(50)]
        public async Task DecreaseBalance(HttpClient client, Account account)
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
        
        [Theory, AccountWithBalanceConventions(50)]
        public async Task ConflictIfCommandIsSentTwice(HttpClient client, Account account)
        {
            // Arrange
            var command = new DebitAccount
            {
                AccountId = account.Id,
                Version = account.Version,
                Amount = -25
            };

            // Act
            await client.PostAsJsonAsync("/debit", command);
            var response = await client.PostAsJsonAsync("/debit", command);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Conflict);
        }
        
        [Theory, AccountWithBalanceConventions(50)]
        public async Task OnlyDebitOnce(HttpClient client, Account account)
        {
            // Arrange
            var command = new DebitAccount
            {
                AccountId = account.Id,
                Version = account.Version,
                Amount = -25
            };

            // Act
            var tasks = Enumerable
                .Range(0, 10)
                .Select(_ => Task.Run(() => client.PostAsJsonAsync("/debit", command)))
                .ToArray();

            await Task.WhenAll(tasks);
            var responses = tasks.Select(task => task.Result).ToArray();

            // Assert
            responses
                .Where(message => message.StatusCode is HttpStatusCode.OK)
                .Should()
                .HaveCount(1);
            
            responses
                .Where(message => message.StatusCode is HttpStatusCode.Conflict)
                .Should()
                .HaveCount(9);
        }
    }
}