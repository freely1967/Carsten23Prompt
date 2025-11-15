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

            Assert.Equal(8.99, catalog.GetPrice("burger"));
            Assert.Equal(3.49, catalog.GetPrice("fries"));
            Assert.Equal(4.99, catalog.GetPrice("shake"));
            Assert.Equal(6.49, catalog.GetPrice("nuggets"));
            Assert.Equal(7.99, catalog.GetPrice("salad"));
            Assert.Equal(0.0, catalog.GetPrice("unknown"));
        }
    }
}
