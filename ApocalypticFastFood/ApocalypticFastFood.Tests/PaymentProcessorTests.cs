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

        facade.ProcessCash(12.34);

        Assert.True(fake.Called);
        Assert.Equal(12.34, fake.Amount);
    }

    private class FakeCash : ICashPaymentProcessor
    {
        public bool Called = false;
        public double Amount = 0.0;

        public void ProcessCash(double amount)
        {
            Called = true;
            Amount = amount;
        }
    }
}
