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
    public double GetDiscount(CustomerContext ctx)
    {
        // Placeholder: VIP discount logic is handled elsewhere - return 0 to keep parity
        return 0.0;
    }
}

public class VipLoyaltyCalculator : ILoyaltyCalculator
{
    public int GetMultiplier(CustomerContext ctx)
    {
        // Preserve the previous odd return value used in the codebase
        return -1;
    }
}
