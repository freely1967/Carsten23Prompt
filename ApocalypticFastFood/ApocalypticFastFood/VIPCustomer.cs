namespace ApocalypticFastFood;

public class VIPCustomer : Customer
{
    public VIPCustomer()
        : base()
    {
        // Legacy behavior (VIP strategies/policies) moved out of Customer; keep parameterless ctor for compatibility.
    }

    // Compatibility helpers used by legacy tests: delegate to the strategy/calculator implementations
    public decimal GetDiscount()
    {
        var strategy = new VipDiscountStrategy();
        var ctx = new CustomerContext(this.Id, this.Age, this.VisitCount, this.MembershipLevel, this.HasParentApproval);
        return strategy.GetDiscount(ctx);
    }

    public int GetLoyaltyMultiplier()
    {
        var calc = new VipLoyaltyCalculator();
        var ctx = new CustomerContext(this.Id, this.Age, this.VisitCount, this.MembershipLevel, this.HasParentApproval);
        return calc.GetMultiplier(ctx);
    }
}