using Xunit;

namespace ApocalypticFastFood.Tests
{
    public class DiscountManagerDiscountProviderTests
    {
        [Fact]
        public void Adapter_UsesDiscountManagerCalculateDiscount()
        {
            // Arrange
            var dm = new ApocalypticFastFood.DiscountManager();
            dm.TotalAmount = 100;
            dm.CustomerType = 1;
            var adapter = new ApocalypticFastFood.DiscountManagerDiscountProvider(dm);

            var ctx = new ApocalypticFastFood.CustomerContext(0, 30, 1, "", false);

            // Act
            var d = adapter.GetDiscount(ctx);

            // Assert
            Assert.IsType<double>(d);
        }
    }
}
