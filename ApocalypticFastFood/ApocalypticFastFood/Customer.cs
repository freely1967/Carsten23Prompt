namespace ApocalypticFastFood;

public class Customer
{
    public virtual double GetDiscount()
    {
        return 0;
    }

    public virtual void MakePurchase()
    {
    }

    public virtual bool CanOrderAlcohol()
    {
        return true;
    }

    public virtual int GetLoyaltyMultiplier()
    {
        return 1;
    }
}