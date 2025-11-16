namespace ApocalypticFastFood;

public class CustomerDiscountProvider : IDiscountProvider
{
    private readonly Customer _customer;

    public CustomerDiscountProvider(Customer customer)
    {
        _customer = customer ?? throw new System.ArgumentNullException(nameof(customer));
    }

    public decimal GetDiscount(CustomerContext ctx)
    {
        // Avoid mutating shared objects. Create transient, initialized instances per call.
        var localCustomer = new Customer
        {
            Id = ctx.Id,
            Age = ctx.Age,
            VisitCount = ctx.VisitCount,
            MembershipLevel = ctx.MembershipLevel ?? string.Empty,
            HasParentApproval = ctx.HasParentApproval
        };

        var dm = new DiscountManager(); // consider factory/DI if dependencies matter
        dm.Id = localCustomer.Id;
        dm.Age = localCustomer.Age;
        dm.VisitCount = localCustomer.VisitCount;
        dm.MembershipLevel = localCustomer.MembershipLevel;
        dm.HasParentApproval = localCustomer.HasParentApproval;

        return dm.CalculateDiscount();
    }
}
