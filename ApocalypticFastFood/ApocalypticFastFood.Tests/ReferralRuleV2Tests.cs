using Xunit;

namespace ApocalypticFastFood.Tests
{
    public class ReferralRuleV2Tests
    {
        [Fact]
        public void ReferralRuleV2_Referral20_VipDiamond_LongMembership_HighAverageSpend()
        {
            var dm = new DiscountManager
            {
                ReferralCount = 25,
                CustomerType = 2,
                MembershipLevel = "Diamond",
                MonthsSinceMembership = 13,
                AverageSpend = 70.0
            };

            var rule = new ReferralRuleV2();
            var res = rule.CalculateResult(dm);

            Assert.Equal(45.0, res.Discount, 2);
            Assert.Equal(1.0, res.Multiplier, 3);
        }

        [Fact]
        public void ReferralRuleV2_Referral10_NonVip_Uses18()
        {
            var dm = new DiscountManager
            {
                ReferralCount = 12,
                CustomerType = 1
            };

            var rule = new ReferralRuleV2();
            var res = rule.CalculateResult(dm);

            Assert.Equal(18.0, res.Discount, 2);
        }

        [Fact]
        public void ReferralRuleV2_ReferralLessThan5_Uses6()
        {
            var dm = new DiscountManager
            {
                ReferralCount = 1,
                CustomerType = 1
            };

            var rule = new ReferralRuleV2();
            var res = rule.CalculateResult(dm);

            Assert.Equal(6.0, res.Discount, 2);
        }
    }
}
