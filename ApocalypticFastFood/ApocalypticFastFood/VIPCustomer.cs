namespace ApocalypticFastFood;

public class VipCustomer : Customer
{
    public override double GetDiscount()
    {
        // VIPs now use an injected discount strategy; keep backward-compatible behavior
        return base.GetDiscount();
    }

    public override int GetLoyaltyMultiplier()
    {
        // Preserve previous odd behavior for now
        return base.GetLoyaltyMultiplier();
    } // Negative? What?
}