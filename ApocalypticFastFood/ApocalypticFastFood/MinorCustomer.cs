namespace ApocalypticFastFood;

public class MinorCustomer : Customer
{
    public override bool CanOrderAlcohol()
    {
        throw new InvalidOperationException("Minors can't order alcohol!");
    }

    public override void MakePurchase()
    {
        throw new Exception("Need parent approval!");
    }
}