using Xunit;
using ApocalypticFastFood;

namespace ApocalypticFastFood.Tests;

public class ReceiptFormatterTests
{
    [Fact]
    public void Format_IncludesTotalLabel()
    {
        var dm = new DiscountManager();
        dm.OrderNumber = 1;
        dm.Day = "Monday";
        dm.Hour = 10;
        dm.Minute = 30;
        dm.RestaurantId = 1;
        dm.ItemQuantities = new System.Collections.Generic.Dictionary<string,int> { ["burger"] = 1 };
        dm.TotalAmount = 8.99m;

        var f = new ReceiptFormatter();
        var s = f.Format(dm);

        Assert.Contains("TOTAL:", s);
    }
}
