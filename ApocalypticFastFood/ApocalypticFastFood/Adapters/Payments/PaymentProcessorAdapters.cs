namespace ApocalypticFastFood;

// Segregated payment interfaces (moved out of DiscountManager.cs)
public interface IPaymentProcessor
{
    void ProcessCreditCard(string cardNum, string cvv, string exp);
    void ProcessDebitCard(string cardNum, string pin);
    void ProcessPaypal(string email, string password);
    void ProcessCrypto(string wallet, string coin);
    void ProcessGiftCard(string code);
    void ProcessCash(decimal amount);
    void ProcessCheck(string checkNum);
    void ProcessBankTransfer(string routing, string account);
}

public interface ICashPaymentProcessor
{
    void ProcessCash(decimal amount);
}

public interface ICardPaymentProcessor
{
    void ProcessCreditCard(string cardNum, string cvv, string exp);
    void ProcessDebitCard(string cardNum, string pin);
}

public interface IPaypalProcessor
{
    void ProcessPaypal(string email, string password);
}

public interface ICryptoProcessor
{
    void ProcessCrypto(string wallet, string coin);
}

public interface IGiftCardProcessor
{
    void ProcessGiftCard(string code);
}

public interface ICheckProcessor
{
    void ProcessCheck(string checkNum);
}

public interface IBankTransferProcessor
{
    void ProcessBankTransfer(string routing, string account);
}

// Backwards-compatible concrete that still implements the legacy interface
public class CashPaymentProcessor : ICashPaymentProcessor
{
    public void ProcessCash(decimal amount)
    {
        Console.WriteLine($"Processing cash: ${amount:F2}");
    }
}

// Simple card payment processor (stub implementation)
public class CardPaymentProcessor : ICardPaymentProcessor
{
    public void ProcessCreditCard(string cardNum, string cvv, string exp)
    {
        Console.WriteLine($"Processing credit card {cardNum} exp {exp}");
    }

    public void ProcessDebitCard(string cardNum, string pin)
    {
        Console.WriteLine($"Processing debit card {cardNum}");
    }
}

// Facade that composes segregated processors and exposes the legacy large interface
public class PaymentProcessorFacade : IPaymentProcessor
{
    private readonly ICashPaymentProcessor? _cash;
    private readonly ICardPaymentProcessor? _card;
    private readonly IPaypalProcessor? _paypal;
    private readonly ICryptoProcessor? _crypto;
    private readonly IGiftCardProcessor? _gift;
    private readonly ICheckProcessor? _check;
    private readonly IBankTransferProcessor? _bank;

    public PaymentProcessorFacade(ICashPaymentProcessor? cash = null,
                                  ICardPaymentProcessor? card = null,
                                  IPaypalProcessor? paypal = null,
                                  ICryptoProcessor? crypto = null,
                                  IGiftCardProcessor? gift = null,
                                  ICheckProcessor? check = null,
                                  IBankTransferProcessor? bank = null)
    {
        _cash = cash;
        _card = card;
        _paypal = paypal;
        _crypto = crypto;
        _gift = gift;
        _check = check;
        _bank = bank;
    }

    public void ProcessCreditCard(string cardNum, string cvv, string exp)
    {
        if (_card is null) throw new NotSupportedException("Card processing not configured");
        _card.ProcessCreditCard(cardNum, cvv, exp);
    }

    public void ProcessDebitCard(string cardNum, string pin)
    {
        if (_card is null) throw new NotSupportedException("Card processing not configured");
        _card.ProcessDebitCard(cardNum, pin);
    }

    public void ProcessPaypal(string email, string password)
    {
        if (_paypal is null) throw new NotSupportedException("Paypal not configured");
        _paypal.ProcessPaypal(email, password);
    }

    public void ProcessCrypto(string wallet, string coin)
    {
        if (_crypto is null) throw new NotSupportedException("Crypto not configured");
        _crypto.ProcessCrypto(wallet, coin);
    }

    public void ProcessGiftCard(string code)
    {
        if (_gift is null) throw new NotSupportedException("Gift card processing not configured");
        _gift.ProcessGiftCard(code);
    }

    public void ProcessCash(decimal amount)
    {
        if (_cash is null) throw new NotSupportedException("Cash processing not configured");
        _cash.ProcessCash(amount);
    }

    public void ProcessCheck(string checkNum)
    {
        if (_check is null) throw new NotSupportedException("Check processing not configured");
        _check.ProcessCheck(checkNum);
    }

    public void ProcessBankTransfer(string routing, string account)
    {
        if (_bank is null) throw new NotSupportedException("Bank transfer not configured");
        _bank.ProcessBankTransfer(routing, account);
    }
}
