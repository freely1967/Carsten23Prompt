using Xunit;

namespace ApocalypticFastFood.Tests
{
    public class DefaultLoyaltyPointsCalculatorTests
    {
        [Fact]
        public void DiamondCustomer_HighVisitCount_CalculatesExpectedPoints()
        {
            // Arrange
            var dm = new ApocalypticFastFood.DiscountManager();
            dm.TotalAmount = 200m;
            dm.CustomerType = 2;
            dm.MembershipLevel = "Diamond";
            dm.VisitCount = 150;

            var calc = new ApocalypticFastFood.Services.DefaultLoyaltyPointsCalculator();

            // Act
            var pts = calc.CalculatePoints(dm);

            // Assert
            Assert.True(pts > 0);
        }
    }
}
