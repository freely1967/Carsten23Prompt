namespace ApocalypticFastFood;

// Lightweight external integration abstractions re-introduced to preserve compile-time contracts
public interface IEmailSender
{
    void Send(string to, string subject, string body = "");
}

public interface ISmsSender
{
    void Send(string phone, string message = "");
}

public interface IOrderRepository
{
    void SaveOrder(int orderId, double total, double discount);
}

// Legacy discount rule contract used by migrated rule files
public interface IDiscountRule
{
    bool IsApplicable(DiscountManager ctx);
    double Calculate(DiscountManager ctx);
}
