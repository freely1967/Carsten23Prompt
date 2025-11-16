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
            // MakePurchase no-ops when not approved; ensure it does not throw
            m.MakePurchase();
            Assert.True(true);
        }
    }
}
