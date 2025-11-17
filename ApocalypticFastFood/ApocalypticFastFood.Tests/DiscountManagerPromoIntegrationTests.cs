using Xunit;
using ApocalypticFastFood;

namespace ApocalypticFastFood.Tests;

public class DiscountManagerPromoIntegrationTests
{
    [Fact]
    public void CalculateDiscount_Uses_PromoEngine_WhenPromoPresent()
    {
        var dm = new DiscountManager { PromoCode = "SAVE10", TotalAmount = 60.0m, CustomerType = CustomerCategory.Regular };
        var engine = new DiscountEngine(new[] { new PromoCodeRule() });

        var expected = engine.Calculate(dm);
        var actual = dm.CalculateDiscount();

        Assert.Equal(expected, actual);
    }
}
