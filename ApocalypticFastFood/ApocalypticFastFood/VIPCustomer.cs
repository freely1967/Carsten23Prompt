namespace ApocalypticFastFood;

public class VipCustomer : Customer
{
    public override double GetDiscount()
    {
        throw new Exception("VIP customers use different discount system!");
    }

    public override int GetLoyaltyMultiplier()
    {
        return -1;
    } // Negative? What?
}