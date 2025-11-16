using Xunit;
using ApocalypticFastFood;

namespace ApocalypticFastFood.Tests;

public class CardPaymentProcessorTests
{
    [Fact]
    public void Facade_Delegates_CreditCard()
    {
        var fake = new FakeCard();
        var facade = new PaymentProcessorFacade(null, fake);

        facade.ProcessCreditCard("4111111111111111", "123", "12/29");

        Assert.True(fake.Called);
        Assert.Equal("4111111111111111", fake.Number);
    }

        private class FakeCard : ICardPaymentProcessor
        {
            public bool Called { get; private set; } = false;
            public string Number { get; private set; } = string.Empty;

            public void ProcessCreditCard(string cardNum, string cvv, string exp)
            {
                Called = true;
                Number = cardNum;
            }

            public void ProcessDebitCard(string cardNum, string pin) { }
        }
}
