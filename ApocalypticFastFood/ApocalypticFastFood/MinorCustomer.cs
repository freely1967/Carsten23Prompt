namespace ApocalypticFastFood;

public class MinorCustomer : Customer
{
    public override bool CanOrderAlcohol()
    {
        // Use policy-based check rather than throwing
        return base.CanOrderAlcohol();
    }

    public override void MakePurchase()
    {
        // Delegate to policy; if not approved, the base implementation logs and returns
        base.MakePurchase();
    }
}