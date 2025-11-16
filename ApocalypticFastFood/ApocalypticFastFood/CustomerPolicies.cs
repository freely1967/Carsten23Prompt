namespace ApocalypticFastFood;

// Minor-specific policy implementations
public class MinorAlcoholPolicy : IAlcoholPolicy
{
    public bool CanOrderAlcohol(CustomerContext ctx)
    {
        // Minors cannot order alcohol
        return ctx.Age >= 18;
    }
}

public class MinorPurchaseApprovalPolicy : IPurchaseApprovalPolicy
{
    public bool CanMakePurchase(CustomerContext ctx)
    {
        // Allow purchase only if parent approval flag is set
        return ctx.HasParentApproval;
    }
}

public class VipDiscountStrategy : IDiscountStrategy
{
    public decimal GetDiscount(CustomerContext ctx)
    {
        // Placeholder: VIP discount logic is handled elsewhere - return 0 to keep parity
        return 0.0m;
    }
}

public class VipLoyaltyCalculator : ILoyaltyCalculator
{
    public int GetMultiplier(CustomerContext ctx)
    {
        // VIP customers: provide a positive, documented multiplier.
        // Using a positive multiplier is less surprising and easier to reason about.
        return 2;
    }
}
