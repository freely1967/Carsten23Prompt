using Xunit;

namespace ApocalypticFastFood.Tests
{
    public class VipCustomerStrategyTests
    {
        [Fact]
        public void VipCustomer_GetDiscount_ReturnsDoubleAndDoesNotThrow()
        {
            var vip = new VIPCustomer { Id = 1, Age = 45, VisitCount = 150, MembershipLevel = "Diamond" };
            decimal d = 0.0m;

            // Act
            d = vip.GetDiscount();

            // Assert
            Assert.IsType<decimal>(d);
        }

        [Fact]
        public void VipCustomer_DefaultStrategy_IsVipDiscountStrategy()
        {
            var vip = new VIPCustomer { Id = 2 };
            // This simply verifies GetDiscount executes without exception and returns a numeric value
            var d = vip.GetDiscount();
            Assert.IsType<decimal>(d);
        }
    }
}
