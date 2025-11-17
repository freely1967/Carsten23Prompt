using Xunit;
using ApocalypticFastFood;
using System.Collections.Generic;

namespace ApocalypticFastFood.Tests;

public class DiscountEngineTests
{
    [Fact]
    public void Engine_Applies_PromoCodeRule_SAVE10()
    {
        var dm = new DiscountManager { PromoCode = "SAVE10", TotalAmount = 60.0m, CustomerType = CustomerCategory.Regular };
        var engine = new DiscountEngine(new List<IDiscountRule> { new PromoCodeRule() });

        var d = engine.Calculate(dm);

        Assert.Equal(10.0m, d);
    }

    [Fact]
    public void Engine_Applies_VisitCountRule_100plus()
    {
        var dm = new DiscountManager { VisitCount = 120, CustomerType = CustomerCategory.VIP, MembershipLevel = "Diamond", MonthsSinceMembership = 36, AverageSpend = 80m, ConsecutiveVisits = 15, LeftReview = true, ReviewStars = 5, ReferralCount = 5 };
        var engine = new DiscountEngine(new List<IDiscountRule> { new VisitCountRule() });

        var d = engine.Calculate(dm);

        Assert.Equal(45.0m, d);
    }
}
