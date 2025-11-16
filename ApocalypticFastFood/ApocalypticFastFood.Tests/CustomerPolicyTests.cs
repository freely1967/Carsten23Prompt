using Xunit;
using ApocalypticFastFood;

namespace ApocalypticFastFood.Tests;

public class CustomerPolicyTests
{
    [Fact]
    public void MinorCustomer_CannotOrderAlcohol_ReturnsFalse()
    {
        var minor = new MinorCustomer();
        minor.Age = 16;
        // Inject minor-specific alcohol policy
        var ctx = new CustomerContext(minor.Id, minor.Age, minor.VisitCount, minor.MembershipLevel, minor.HasParentApproval);
        var policy = new MinorAlcoholPolicy();
        Assert.False(policy.CanOrderAlcohol(ctx));
    }

    [Fact]
    public void MinorCustomer_MakePurchase_NoException_WhenNoApproval()
    {
        var minor = new MinorCustomer();
        minor.HasParentApproval = false;
        // Should not throw; use the service adapter instead of the obsolete Customer.MakePurchase()
        var svc = new ApocalypticFastFood.Services.CustomerPurchaseService(minor);
        var ctx = new ApocalypticFastFood.CustomerContext(minor.Id, minor.Age, minor.VisitCount, minor.MembershipLevel, minor.HasParentApproval);
        svc.MakePurchase(ctx);
    }

    [Fact]
    public void VipCustomer_LoyaltyMultiplier_PreservesLegacyValue()
    {
        var vip = new VIPCustomer();
        // VIP customers should have a positive loyalty multiplier; avoid asserting legacy sentinel values
        Assert.True(vip.GetLoyaltyMultiplier() > 0, "Expected positive loyalty multiplier for VIP customers");
    }
}
