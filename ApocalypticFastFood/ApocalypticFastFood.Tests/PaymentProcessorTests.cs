using Xunit;
using ApocalypticFastFood;

namespace ApocalypticFastFood.Tests;

public class PaymentProcessorTests
{
    [Fact]
    public void Facade_Delegates_ProcessCash()
    {
        var fake = new FakeCash();
        var facade = new PaymentProcessorFacade(fake);

        facade.ProcessCash(12.34m);

        Assert.True(fake.Called);
        Assert.Equal(12.34m, fake.Amount);
    }

    private class FakeCash : ICashPaymentProcessor
    {
        public bool Called { get; private set; } = false;
        public decimal Amount { get; private set; } = 0.0m;

        public void ProcessCash(decimal amount)
        {
            Called = true;
            Amount = amount;
        }
    }
}
