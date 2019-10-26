using AutoFixture;
using AutoFixture.Xunit2;
using SimpleInjector;
using Tests.Primitives;

namespace Tests.Async
{
    public class AccountWithBalanceConventions : AutoDataAttribute
    {
        public AccountWithBalanceConventions(int value) : base(() => Create(value))
        {
        }

        private static IFixture Create(int value)
        {
            var container = new Container();
            var fixture = new Fixture()
                .Container(container)
                .Async()
                .Customize(new TestServerCustomization(container))
                .Customize(new ExistingAccountWithBalanceCustomization(value));
            return fixture;
        }
    }
}