namespace ApocalypticFastFood;

public class CustomerDiscountProvider : IDiscountProvider
{
    private readonly Customer _customer;
    private readonly DiscountManager _dm;

    public CustomerDiscountProvider(Customer customer)
    {
        _customer = customer ?? throw new System.ArgumentNullException(nameof(customer));
        _dm = new DiscountManager();
    }

    public double GetDiscount(CustomerContext ctx)
    {
        // Apply context to the wrapped Customer (data holder)
        _customer.Id = ctx.Id;
        _customer.Age = ctx.Age;
        _customer.VisitCount = ctx.VisitCount;
        _customer.MembershipLevel = ctx.MembershipLevel ?? string.Empty;
        _customer.HasParentApproval = ctx.HasParentApproval;

        // Map relevant fields into a DiscountManager instance and delegate calculation
        _dm.Id = _customer.Id;
        _dm.Age = _customer.Age;
        _dm.VisitCount = _customer.VisitCount;
        _dm.MembershipLevel = _customer.MembershipLevel;
        _dm.HasParentApproval = _customer.HasParentApproval;

        return _dm.CalculateDiscount();
    }
}
