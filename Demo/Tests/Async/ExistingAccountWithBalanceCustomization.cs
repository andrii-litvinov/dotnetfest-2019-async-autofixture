using AutoFixture;
using Domain;
using MongoDB.Bson;
using Tests.Primitives;

namespace Tests.Async
{
    class ExistingAccountWithBalanceCustomization : ICustomization
    {
        private readonly int value;

        public ExistingAccountWithBalanceCustomization(int value)
        {
            this.value = value;
        }

        public void Customize(IFixture fixture)
        {
            var account = Account.Create(ObjectId.GenerateNewId().ToString());
            account.Credit(value, account.Version);
            fixture.Inject(account);

            fixture.RegisterInitializer<AccountInitializer>();
        }
    }
}