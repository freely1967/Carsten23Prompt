using Xunit;
using ApocalypticFastFood;
using System.Collections.Generic;

namespace ApocalypticFastFood.Tests;

public class OrderProcessorPaymentTests
{
    [Fact]
    public void OrderProcessor_Uses_PaymentProcessor_ForCash()
    {
        // Arrange
        var fakePayment = new FakePaymentProcessor();
        var dm = new DiscountManager { PaymentMethod = "cash" };
        var orderProcessor = new OrderProcessor(null, null, dm, null, null, fakePayment);

        // Act
        orderProcessor.ProcessOrder(1, "Monday", 10, new List<string> { "burger" });

        // Assert
        Assert.True(fakePayment.CashCalled);
        Assert.Equal(dm.TotalAmount, fakePayment.CashAmount);
    }

    private class FakePaymentProcessor : IPaymentProcessor
    {
        public bool CashCalled = false;
        public double CashAmount = 0.0;

        public void ProcessCash(double amount)
        {
            CashCalled = true;
            CashAmount = amount;
        }

        public void ProcessCreditCard(string cardNum, string cvv, string exp) { }
        public void ProcessDebitCard(string cardNum, string pin) { }
        public void ProcessPaypal(string email, string password) { }
        public void ProcessCrypto(string wallet, string coin) { }
        public void ProcessGiftCard(string code) { }
        public void ProcessCheck(string checkNum) { }
        public void ProcessBankTransfer(string routing, string account) { }
    }
}
