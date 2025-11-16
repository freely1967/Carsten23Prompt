namespace ApocalypticFastFood;

public class DiscountManagerDiscountProvider : IDiscountProvider
{
    private readonly DiscountManager _dm;

    public DiscountManagerDiscountProvider(DiscountManager dm)
    {
        _dm = dm ?? throw new ArgumentNullException(nameof(dm));
    }

    public double GetDiscount(CustomerContext ctx)
    {
        // Map available context fields into the existing DiscountManager instance
        _dm.Id = ctx.Id;
        _dm.Age = ctx.Age;
        _dm.VisitCount = ctx.VisitCount;
        _dm.MembershipLevel = ctx.MembershipLevel ?? string.Empty;
        _dm.HasParentApproval = ctx.HasParentApproval;

        return _dm.CalculateDiscount();
    }
}
