namespace ApocalypticFastFood.Adapters;

public class DiscountManagerDiscountProvider : ApocalypticFastFood.IDiscountProvider
{
    private readonly ApocalypticFastFood.DiscountManager _dm;

    public DiscountManagerDiscountProvider(ApocalypticFastFood.DiscountManager dm)
    {
        _dm = dm ?? throw new ArgumentNullException(nameof(dm));
    }

    public double GetDiscount(ApocalypticFastFood.CustomerContext ctx)
    {
        // Map available context fields into the existing DiscountManager instance
        _dm.Age = ctx.Age;
        _dm.VisitCount = ctx.VisitCount;
        _dm.MembershipLevel = ctx.MembershipLevel ?? string.Empty;
        _dm.HasParentApproval = ctx.HasParentApproval;

        return _dm.CalculateDiscount();
    }
}
