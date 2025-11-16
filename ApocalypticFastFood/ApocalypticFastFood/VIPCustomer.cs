namespace ApocalypticFastFood;

public class VipCustomer : Customer
{
    public VipCustomer() : base(discountStrategy: new VipDiscountStrategy())
    {
    }

    // Compatibility helpers used by legacy tests: delegate to the strategy/calculator implementations
    public double GetDiscount()
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