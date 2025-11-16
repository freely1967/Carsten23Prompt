using Xunit;
using ApocalypticFastFood;

namespace ApocalypticFastFood.Tests
{
    public class PriceCatalogTests
    {
        [Fact]
        public void InMemoryPriceCatalog_ReturnsExpectedPrices()
        {
            var catalog = new InMemoryPriceCatalog();

            Assert.Equal(8.99m, catalog.GetPrice("burger"));
            Assert.Equal(3.49m, catalog.GetPrice("fries"));
            Assert.Equal(4.99m, catalog.GetPrice("shake"));
            Assert.Equal(6.49m, catalog.GetPrice("nuggets"));
            Assert.Equal(7.99m, catalog.GetPrice("salad"));
            Assert.Equal(0.0m, catalog.GetPrice("unknown"));
        }
    }
}
