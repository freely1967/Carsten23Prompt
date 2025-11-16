namespace ApocalypticFastFood;

public class MinorCustomer : Customer
{
    public MinorCustomer()
        : base(discountStrategy: null, alcoholPolicy: new MinorAlcoholPolicy(), purchaseApprovalPolicy: new MinorApprovalPolicy())
    {
    }
}