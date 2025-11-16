using Xunit;

namespace ApocalypticFastFood.Tests
{
    public class CustomerDiscountProviderTests
    {
        [Fact]
        public void CustomerDiscountProvider_ReturnsDouble_AndDoesNotThrow()
        {
            var c = new Customer { Id = 1, Age = 30, VisitCount = 5 };
            var provider = new CustomerDiscountProvider(c);
            var ctx = new CustomerContext(1, 30, 5, "", false);

            var d = provider.GetDiscount(ctx);
            Assert.IsType<double>(d);
        }
    }
}
