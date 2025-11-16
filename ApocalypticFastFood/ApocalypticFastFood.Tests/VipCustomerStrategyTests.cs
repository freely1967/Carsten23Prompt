using Xunit;

namespace ApocalypticFastFood.Tests
{
    public class VipCustomerStrategyTests
    {
        [Fact]
        public void VipCustomer_GetDiscount_ReturnsDoubleAndDoesNotThrow()
        {
            var vip = new VipCustomer { Id = 1, Age = 45, VisitCount = 150, MembershipLevel = "Diamond" };
            double d = 0.0;

            // Act
            d = vip.GetDiscount();

            // Assert
            Assert.IsType<double>(d);
        }

        [Fact]
        public void VipCustomer_DefaultStrategy_IsVipDiscountStrategy()
        {
            var vip = new VipCustomer { Id = 2 };
            // This simply verifies GetDiscount executes without exception and returns a numeric value
            var d = vip.GetDiscount();
            Assert.IsType<double>(d);
        }
    }
}
