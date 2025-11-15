using Xunit;

namespace ApocalypticFastFood.Tests
{
    public class BirthdayRuleTests
    {
        [Fact]
        public void BirthdayRule_VipDiamondHighVisit_ReturnsExpected()
        {
            var dm = new DiscountManager
            {
                IsBirthday = true,
                CustomerType = 2,
                MembershipLevel = "Diamond",
                VisitCount = 101,
                TotalAmount = 150.0,
                FamilyMembers = 3
            };

            var rule = new BirthdayRule();
            var res = rule.CalculateResult(dm);

            Assert.Equal(70.0, res.Discount);
            Assert.Equal(1.6, res.Multiplier, 3);
        }

        [Fact]
        public void BirthdayRule_NonVip_ReturnsExpected()
        {
            var dm = new DiscountManager
            {
                IsBirthday = true,
                CustomerType = 1
            };

            var rule = new BirthdayRule();
            var res = rule.CalculateResult(dm);

            Assert.Equal(25.0, res.Discount);
            Assert.Equal(1.15, res.Multiplier, 3);
        }
    }
}
