using Xunit;
using ApocalypticFastFood;
using System.Collections.Generic;

namespace ApocalypticFastFood.Tests;

public class OrderProcessorEmailTests
{
    [Fact]
    public void OrderProcessor_SavesOrder_And_SendsEmail_WhenSubscribed()
    {
        var fakeRepo = new FakeRepo();
        var fakeEmail = new FakeEmailSender();
        var dm = new DiscountManager { PaymentMethod = "cash", EmailSubscribed = true };
        var op = new OrderProcessor(null, fakeRepo, dm, fakeEmail, null, new FakePaymentProcessor());

        op.ProcessOrder(1, "Monday", 10, new List<string> { "burger" });

        Assert.True(fakeRepo.Saved);
        Assert.True(fakeEmail.Sent);
    }

    private class FakeRepo : IOrderRepository
    {
        public bool Saved = false;
        public void SaveOrder(int orderId, double total, double discount) => Saved = true;
    }

    private class FakeEmailSender : IEmailSender
    {
        public bool Sent = false;
        public void Send(string to, string subject, string body = "") => Sent = true;
    }

    private class FakePaymentProcessor : IPaymentProcessor
    {
        public bool CashCalled = false;
        public double CashAmount = 0.0;

        public void ProcessCash(double amount) { CashCalled = true; CashAmount = amount; }
        public void ProcessCreditCard(string cardNum, string cvv, string exp) { }
        public void ProcessDebitCard(string cardNum, string pin) { }
        public void ProcessPaypal(string email, string password) { }
        public void ProcessCrypto(string wallet, string coin) { }
        public void ProcessGiftCard(string code) { }
        public void ProcessCheck(string checkNum) { }
        public void ProcessBankTransfer(string routing, string account) { }
    }
}
