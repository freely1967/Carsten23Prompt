namespace ApocalypticFastFood;

public class VipCustomer : Customer
{
    public VipCustomer() : base(discountStrategy: new VipDiscountStrategy())
    {
    }
}