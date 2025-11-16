using System.Collections.Generic;
using Xunit;

namespace ApocalypticFastFood.Tests
{
    public class OrderProcessorTests
    {
        private class TestDiscountProvider : ApocalypticFastFood.IDiscountProvider
        {
            private readonly double _value;
            public TestDiscountProvider(double value) => _value = value;
            public double GetDiscount(ApocalypticFastFood.CustomerContext ctx) => _value;
        }

        private class FakeOrderRepo : ApocalypticFastFood.IOrderRepository
        {
            public int SavedOrderId;
            public double SavedTotal;
            public double SavedDiscount;
            public void SaveOrder(int orderId, double total, double discount)
            {
                SavedOrderId = orderId;
                SavedTotal = total;
                SavedDiscount = discount;
            }
        }

        [Fact]
        public void ProcessOrder_UsesProvidedDiscountProvider_WhenPresent()
        {
            // Arrange
            var priceCatalog = new ApocalypticFastFood.InMemoryPriceCatalog();
            var fakeRepo = new FakeOrderRepo();
            var dm = new ApocalypticFastFood.DiscountManager();
            var provider = new TestDiscountProvider(7.5);

            var proc = new ApocalypticFastFood.OrderProcessor(priceCatalog, fakeRepo, dm, null, null, null, provider);

            // Act
            proc.ProcessOrder(1, "Monday", 12, new List<string> { "burger", "fries" });

            // Assert
            Assert.Equal(7.5, fakeRepo.SavedDiscount);
        }
    }
}
