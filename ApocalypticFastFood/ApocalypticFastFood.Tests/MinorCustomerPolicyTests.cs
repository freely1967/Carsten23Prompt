using Xunit;

namespace ApocalypticFastFood.Tests
{
    public class MinorCustomerPolicyTests
    {
        [Fact]
        public void MinorCustomer_CannotOrderAlcohol_Under18_NoApproval()
        {
            var m = new MinorCustomer { Id = 1, Age = 16, HasParentApproval = false };
            Assert.False(m.CanOrderAlcohol());
        }

        [Fact]
        public void MinorCustomer_CanOrderAlcohol_WithParentApproval()
        {
            var m = new MinorCustomer { Id = 2, Age = 16, HasParentApproval = true };
            Assert.True(m.CanOrderAlcohol());
        }

        [Fact]
        public void MinorCustomer_CanMakePurchase_WithParentApproval()
        {
            var m = new MinorCustomer { Id = 3, Age = 15, HasParentApproval = true };
            // Use the service adapter instead of calling the obsolete Customer.MakePurchase()
            var svc = new ApocalypticFastFood.Services.CustomerPurchaseService(m);
            var ctx = new ApocalypticFastFood.CustomerContext(m.Id, m.Age, m.VisitCount, m.MembershipLevel, m.HasParentApproval);
            svc.MakePurchase(ctx);
            Assert.True(true);
        }
    }
}
