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
        // Should not throw, base implementation logs and returns
        minor.MakePurchase();
    }

    [Fact]
    public void VipCustomer_LoyaltyMultiplier_PreservesLegacyValue()
    {
        var vip = new VipCustomer();
        Assert.Equal(-1, vip.GetLoyaltyMultiplier());
    }
}
