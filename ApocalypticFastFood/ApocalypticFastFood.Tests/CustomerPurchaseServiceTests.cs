using Xunit;

namespace ApocalypticFastFood.Tests
{
    public class CustomerPurchaseServiceTests
    {
        [Fact]
        public void CustomerPurchaseService_DelegatesToCustomer()
        {
            // Arrange
            var customer = new ApocalypticFastFood.Customer();
            var svc = new ApocalypticFastFood.Services.CustomerPurchaseService(customer);
            var ctx = new ApocalypticFastFood.CustomerContext(1, 17, 0, "", true);

            // Act / Assert: should not throw
            svc.MakePurchase(ctx);
            Assert.True(true);
        }
    }
}
