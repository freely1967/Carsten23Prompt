namespace ApocalypticFastFood;

// Small data holder used to pass customer-related state into policies/strategies
public record CustomerContext(int Id = 0, int Age = 0, int VisitCount = 0, string MembershipLevel = "", bool HasParentApproval = false);

public interface IDiscountStrategy
{
    double GetDiscount(CustomerContext ctx);
}

public interface IAlcoholPolicy
{
    bool CanOrderAlcohol(CustomerContext ctx);
}

public interface IPurchaseApprovalPolicy
{
    bool CanMakePurchase(CustomerContext ctx);
}

public interface ILoyaltyCalculator
{
    int GetMultiplier(CustomerContext ctx);
}

// Default implementations that preserve original behaviors
public class DefaultDiscountStrategy : IDiscountStrategy
{
    public double GetDiscount(CustomerContext ctx) => 0.0;
}

public class DefaultAlcoholPolicy : IAlcoholPolicy
{
    // Original Customer.CanOrderAlcohol returned true by default
    public bool CanOrderAlcohol(CustomerContext ctx) => true;
}

public class DefaultPurchaseApprovalPolicy : IPurchaseApprovalPolicy
{
    public bool CanMakePurchase(CustomerContext ctx) => true;
}

public class DefaultLoyaltyCalculator : ILoyaltyCalculator
{
    public int GetMultiplier(CustomerContext ctx) => 1;
}

public class Customer
{
    // lightweight state kept for backwards compatibility
    public int Id { get; set; }
    public int Age { get; set; }
    public int VisitCount { get; set; }
    public string MembershipLevel { get; set; } = string.Empty;
    public bool HasParentApproval { get; set; }

    private readonly IDiscountStrategy _discountStrategy;
    private readonly IAlcoholPolicy _alcoholPolicy;
    private readonly IPurchaseApprovalPolicy _purchaseApprovalPolicy;
    private readonly ILoyaltyCalculator _loyaltyCalculator;

    public Customer(IDiscountStrategy? discountStrategy = null,
                    IAlcoholPolicy? alcoholPolicy = null,
                    IPurchaseApprovalPolicy? purchaseApprovalPolicy = null,
                    ILoyaltyCalculator? loyaltyCalculator = null)
    {
        _discountStrategy = discountStrategy ?? new DefaultDiscountStrategy();
        _alcoholPolicy = alcoholPolicy ?? new DefaultAlcoholPolicy();
        _purchaseApprovalPolicy = purchaseApprovalPolicy ?? new DefaultPurchaseApprovalPolicy();
        _loyaltyCalculator = loyaltyCalculator ?? new DefaultLoyaltyCalculator();
    }

    // Backwards-compatible method signatures: delegate to injected policies using current state
    public virtual double GetDiscount()
    {
        var ctx = new CustomerContext(Id, Age, VisitCount, MembershipLevel, HasParentApproval);
        return _discountStrategy.GetDiscount(ctx);
    }

    public virtual void MakePurchase()
    {
        var ctx = new CustomerContext(Id, Age, VisitCount, MembershipLevel, HasParentApproval);
        if (!_purchaseApprovalPolicy.CanMakePurchase(ctx))
        {
            // Previously this threw in some subclasses; new pattern uses policy checks instead of exceptions
            Console.WriteLine("Purchase not approved for customer " + Id);
            return;
        }

        // purchase side-effects would happen here in real implementation
    }

    public virtual bool CanOrderAlcohol()
    {
        var ctx = new CustomerContext(Id, Age, VisitCount, MembershipLevel, HasParentApproval);
        return _alcoholPolicy.CanOrderAlcohol(ctx);
    }

    public virtual int GetLoyaltyMultiplier()
    {
        var ctx = new CustomerContext(Id, Age, VisitCount, MembershipLevel, HasParentApproval);
        return _loyaltyCalculator.GetMultiplier(ctx);
    }
}