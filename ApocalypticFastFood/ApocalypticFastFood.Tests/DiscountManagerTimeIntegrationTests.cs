using Xunit;
using ApocalypticFastFood;
using System.Collections.Generic;

namespace ApocalypticFastFood.Tests;

public class DiscountManagerTimeIntegrationTests
{
    [Fact]
    public void CalculateDiscountUsesTimeEngineForAfternoonCase()
    {
        var dm = new DiscountManager
        {
            Hour = 15,
            Minute = 10,
            CustomerType = CustomerCategory.Regular,
            IsDineIn = true,
            Items = new List<string> { "burger" },
            VisitCount = 5
        };

        var engine = new DiscountEngine(new IDiscountRule[] { new TimeOfDayRule() });
        var expected = engine.Calculate(dm);
        var actual = dm.CalculateDiscount();

        Assert.Equal(expected, actual);
    }
}
