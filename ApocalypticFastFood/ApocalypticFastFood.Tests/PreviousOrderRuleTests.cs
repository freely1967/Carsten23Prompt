using Xunit;

namespace ApocalypticFastFood.Tests
{
    public class PreviousOrderRuleTests
    {
        [Fact]
        public void PreviousOrderRule_MatchesRecentOrder_VipDiamond_AppAndEmail_ProducesMultiplier()
        {
            var dm = new DiscountManager
            {
                Items = new System.Collections.Generic.List<string> { "burger", "fries" },
                PreviousOrder = "burger,fries",
                DaysLastVisit = 2,
                ConsecutiveVisits = 5,
                CustomerType = CustomerCategory.VIP,
                MembershipLevel = "Diamond",
                AverageSpend = 100.0m,
                HasApp = true,
                EmailSubscribed = true
            };

            var rule = new PreviousOrderRule();
            var res = rule.CalculateResult(dm);

            Assert.Equal(35.0m, res.Discount);
            Assert.Equal(1.25, res.Multiplier, 3);
        }

        [Fact]
        public void PreviousOrderRule_NoRecentMatch_ReturnsZero()
        {
            var dm = new DiscountManager
            {
                Items = new System.Collections.Generic.List<string> { "burger" },
                PreviousOrder = "fries",
                DaysLastVisit = 10
            };

            var rule = new PreviousOrderRule();
            var res = rule.CalculateResult(dm);

            Assert.Equal(0.0m, res.Discount);
            Assert.Equal(1.0, res.Multiplier, 3);
        }
    }
}
