using Xunit;

namespace ApocalypticFastFood.Tests
{
    public class VisitCountRuleV2Tests
    {
        [Fact]
        public void VisitCountRuleV2_HighVisit_WithReferralAndDiamond_Multiplier150()
        {
            var dm = new DiscountManager
            {
                VisitCount = 150,
                CustomerType = CustomerCategory.VIP,
                MembershipLevel = "Diamond",
                MonthsSinceMembership = 25,
                AverageSpend = 80.0m,
                ConsecutiveVisits = 11,
                LeftReview = true,
                ReviewStars = 5,
                ReferralCount = 11
            };

            var rule = new VisitCountRuleV2();
            var res = rule.CalculateResult(dm);

            Assert.Equal(1.5, res.Multiplier, 3);
            Assert.Equal(0.0m, res.Discount);
        }

        [Fact]
        public void VisitCountRuleV2_HighVisit_NoReferral_Multiplier140()
        {
            var dm = new DiscountManager
            {
                VisitCount = 120,
                CustomerType = CustomerCategory.VIP,
                MembershipLevel = "Diamond",
                MonthsSinceMembership = 30,
                AverageSpend = 90.0m,
                ConsecutiveVisits = 12,
                LeftReview = true,
                ReviewStars = 5,
                ReferralCount = 5
            };

            var rule = new VisitCountRuleV2();
            var res = rule.CalculateResult(dm);

            Assert.Equal(1.4, res.Multiplier, 3);
        }
    }
}
