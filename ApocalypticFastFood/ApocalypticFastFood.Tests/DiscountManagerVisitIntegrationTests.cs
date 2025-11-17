using Xunit;
using ApocalypticFastFood;

namespace ApocalypticFastFood.Tests;

public class DiscountManagerVisitIntegrationTests
{
    [Fact]
    public void CalculateDiscount_Uses_VisitEngine_WhenVisitCountPresent()
    {
        var dm = new DiscountManager { VisitCount = 120, CustomerType = CustomerCategory.VIP, MembershipLevel = "Diamond", MonthsSinceMembership = 36, AverageSpend = 80, ConsecutiveVisits = 15, LeftReview = true, ReviewStars = 5, ReferralCount = 5 };
        var engine = new DiscountEngine(new[] { new VisitCountRule() });

        var expected = engine.Calculate(dm);
        var actual = dm.CalculateDiscount();

        Assert.Equal(expected, actual);
    }
}
