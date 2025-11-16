namespace ApocalypticFastFood;

public class DiscountManagerDiscountProvider : IDiscountProvider
{
    private readonly DiscountManager _dm;

    public DiscountManagerDiscountProvider(DiscountManager dm)
    {
        _dm = dm ?? throw new System.ArgumentNullException(nameof(dm));
    }

    public decimal GetDiscount(CustomerContext ctx)
    {
        // Avoid mutating consumer-provided DiscountManager. Create a fresh instance for calculation.
        var copy = new DiscountManager(); // or use factory/DI if context-specific dependencies are required
        copy.Id = ctx.Id;
        copy.Age = ctx.Age;
        copy.VisitCount = ctx.VisitCount;
        copy.MembershipLevel = ctx.MembershipLevel ?? string.Empty;
        copy.HasParentApproval = ctx.HasParentApproval;

        return copy.CalculateDiscount();
    }
}
