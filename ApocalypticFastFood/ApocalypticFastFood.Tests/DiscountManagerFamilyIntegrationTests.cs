using Xunit;
using ApocalypticFastFood;

namespace ApocalypticFastFood.Tests;

public class DiscountManagerFamilyIntegrationTests
{
    [Fact]
    public void CalculateDiscount_Uses_FamilyEngine_WhenFamilyPresent()
    {
        var dm = new DiscountManager { FamilyMembers = 5, HasKids = true, ItemCount = 8, Day = "Sunday", Hour = 13, TotalAmount = 90.0m, IsHoliday = true };
        var engine = new DiscountEngine(new[] { new FamilyRule() });

        var expected = engine.Calculate(dm);
        var actual = dm.CalculateDiscount();

        Assert.Equal(expected, actual);
    }
}
