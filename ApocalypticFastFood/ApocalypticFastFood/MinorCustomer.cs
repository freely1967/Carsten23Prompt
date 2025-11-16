namespace ApocalypticFastFood;

public class MinorCustomer : Customer
{
    public MinorCustomer()
        : base()
    {
        // Legacy behavior (policies) moved out of Customer; keep constructor parameterless for compatibility.
    }

    public bool CanOrderAlcohol()
    {
        var policy = new MinorAlcoholPolicy();
        var ctx = new CustomerContext(this.Id, this.Age, this.VisitCount, this.MembershipLevel, this.HasParentApproval);
        // Allow ordering when parent approval is present, otherwise defer to age-based policy
        return this.HasParentApproval || policy.CanOrderAlcohol(ctx);
    }
}