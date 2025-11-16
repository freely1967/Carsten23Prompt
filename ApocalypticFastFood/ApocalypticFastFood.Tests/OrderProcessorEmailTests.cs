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
        var dm = new DiscountManager { PaymentMethodEnum = ApocalypticFastFood.PaymentMethod.Cash, EmailSubscribed = true };
        var op = new OrderProcessor(null, fakeRepo, dm, fakeEmail, null, new FakePaymentProcessor());

        op.ProcessOrder(1, "Monday", 10, new List<string> { "burger" });

        Assert.True(fakeRepo.Saved);
        Assert.True(fakeEmail.Sent);
    }

    private class FakeRepo : IOrderRepository
    {
        public bool Saved { get; private set; } = false;
        public void SaveOrder(int orderId, decimal total, decimal discount) => Saved = true;
    }

    private class FakeEmailSender : IEmailSender
    {
        public bool Sent { get; private set; } = false;
        public void Send(string to, string subject, string body = "") => Sent = true;
    }

    private class FakePaymentProcessor : IPaymentProcessor
    {
        public bool CashCalled { get; private set; } = false;
        public decimal CashAmount { get; private set; } = 0.0m;

        public void ProcessCash(decimal amount) { CashCalled = true; CashAmount = amount; }
        public void ProcessCreditCard(string cardNum, string cvv, string exp) { }
        public void ProcessDebitCard(string cardNum, string pin) { }
        public void ProcessPaypal(string email, string password) { }
        public void ProcessCrypto(string wallet, string coin) { }
        public void ProcessGiftCard(string code) { }
        public void ProcessCheck(string checkNum) { }
        public void ProcessBankTransfer(string routing, string account) { }
    }
}
