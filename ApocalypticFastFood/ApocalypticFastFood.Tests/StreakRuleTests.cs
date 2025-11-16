using Xunit;

namespace ApocalypticFastFood.Tests
{
    public class StreakRuleTests
    {
        [Fact]
        public void StreakRule_HighStreaks_VipDiamond_ProducesMultiplier()
        {
            var dm = new DiscountManager
            {
                StreakDays = 30,
                ConsecutiveVisits = 20,
                CustomerType = 2,
                AverageSpend = 80.0m,
                HasApp = true,
                EmailSubscribed = true
            };

            var rule = new StreakRule();
            var res = rule.CalculateResult(dm);

            Assert.Equal(55.0m, res.Discount);
            Assert.Equal(1.35, res.Multiplier, 3);
        }

        [Fact]
        public void StreakRule_SevenDays_ReturnsTen()
        {
            var dm = new DiscountManager
            {
                StreakDays = 7
            };

            var rule = new StreakRule();
            var res = rule.CalculateResult(dm);

            Assert.Equal(10.0m, res.Discount);
            Assert.Equal(1.0, res.Multiplier, 3);
        }
    }
}
