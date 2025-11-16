using System;
using System.Collections.Generic;

namespace ApocalypticFastFood.Samples;

public static class SampleRunner
{
    public static void RunDemos()
    {
        Console.WriteLine("=== APOCALYPTIC DISCOUNT SYSTEM ===\n");

        // Test 1: VIP Diamond member with all strategies
        var dm1 = new DiscountManager();
        dm1.CustomerType = 2;
        dm1.MembershipLevel = "Diamond";
        dm1.Day = "Tuesday";
        dm1.Hour = 14;
        dm1.Minute = 15;
        dm1.Items = new List<string> { "burger", "burger", "burger", "fries", "fries", "shake", "shake" };
        dm1.ItemQuantities["burger"] = 3;
        dm1.ItemQuantities["fries"] = 2;
        dm1.ItemQuantities["shake"] = 2;
        dm1.TotalAmount = 41.91m;
        dm1.VisitCount = 150;
        dm1.IsBirthday = true;
        dm1.HasApp = true;
        dm1.EmailSubscribed = true;
        dm1.SmsSubscribed = true;
        dm1.Weather = "rainy";
        dm1.Temperature = 55;
        dm1.IsDriveThru = true;
        dm1.ReferralCount = 12;
        dm1.StreakDays = 45;
        dm1.ConsecutiveVisits = 25;
        dm1.AverageSpend = 85m;
        dm1.PromoCode = "VIP50";
        dm1.SocialMediaFollow = "instagram";
        dm1.LeftReview = true;
        dm1.ReviewStars = 5;
        dm1.PaymentMethodEnum = PaymentMethod.App;
        dm1.OrderNumber = 1001;
        dm1.RestaurantId = 101;

        Console.WriteLine("TEST 1: VIP Diamond - Maximum Discounts");
        Console.WriteLine(dm1.GenerateReceipt());
        Console.WriteLine("\n");

        // Test 2: Student on Monday evening
        var dm2 = new DiscountManager();
        dm2.CustomerType = 5;
        dm2.Day = "Monday";
        dm2.Hour = 16;
        dm2.Minute = 30;
        dm2.Age = 20;
        dm2.Items = new List<string> { "burger", "fries", "shake" };
        dm2.ItemQuantities["burger"] = 1;
        dm2.ItemQuantities["fries"] = 1;
        dm2.ItemQuantities["shake"] = 1;
        dm2.TotalAmount = 17.47m;
        dm2.VisitCount = 8;
        dm2.HasApp = true;
        dm2.PromoCode = "STUDENT25";
        dm2.OrderNumber = 1002;
        dm2.RestaurantId = 102;

        Console.WriteLine("TEST 2: Student - Multiple Strategy Stack");
        Console.WriteLine(dm2.GenerateReceipt());
        Console.WriteLine("\n");

        // Test 3: Regular customer rush hour penalty
        var dm3 = new DiscountManager();
        dm3.CustomerType = 1;
        dm3.Day = "Friday";
        dm3.Hour = 12;
        dm3.Minute = 30;
        dm3.Items = new List<string> { "burger", "fries" };
        dm3.ItemQuantities["burger"] = 1;
        dm3.ItemQuantities["fries"] = 1;
        dm3.TotalAmount = 12.48m;
        dm3.IsRushHour = true;
        dm3.IsDriveThru = true;
        dm3.VisitCount = 3;
        dm3.OrderNumber = 1003;
        dm3.RestaurantId = 103;

        Console.WriteLine("TEST 3: Regular Customer - Rush Hour Penalty");
        Console.WriteLine(dm3.GenerateReceipt());
        Console.WriteLine("\n");

        // Test 4: Family meal deal
        var dm4 = new DiscountManager();
        dm4.CustomerType = 1;
        dm4.Day = "Sunday";
        dm4.Hour = 13;
        dm4.FamilyMembers = 5;
        dm4.HasKids = true;
        dm4.Items = new List<string>
            { "burger", "burger", "burger", "burger", "fries", "fries", "fries", "shake", "shake", "nuggets" };
        dm4.ItemQuantities["burger"] = 4;
        dm4.ItemQuantities["fries"] = 3;
        dm4.ItemQuantities["shake"] = 2;
        dm4.ItemQuantities["nuggets"] = 1;
        dm4.TotalAmount = 62.91m;
        dm4.IsHoliday = true;
        dm4.OrderNumber = 1004;
        dm4.RestaurantId = 101;

        Console.WriteLine("TEST 4: Family Meal - Holiday Special");
        Console.WriteLine(dm4.GenerateReceipt());

        // Demonstrate LSP violation examples (behavior preserved for legacy demo)
        Console.WriteLine("\n=== LSP VIOLATION EXAMPLES ===\n");

        try
        {
            var regular = new Customer();
            var regularProvider = new ApocalypticFastFood.CustomerDiscountProvider(regular);
            Console.WriteLine("Regular customer discount: $" + regularProvider.GetDiscount(new ApocalypticFastFood.CustomerContext()));

            Customer vip = new VIPCustomer();
            var vipProvider = new ApocalypticFastFood.CustomerDiscountProvider((Customer)vip);
            Console.WriteLine("VIP customer discount: $" + vipProvider.GetDiscount(new ApocalypticFastFood.CustomerContext()));
        }
        catch (Exception ex)
        {
            Console.WriteLine("LSP VIOLATED! " + ex.Message);
        }

        try
        {
            Customer minor = new MinorCustomer();
            // Use the purchase service adapter to demonstrate staged migration
            var purchaseService = new ApocalypticFastFood.Services.CustomerPurchaseService(minor);
            purchaseService.MakePurchase(new ApocalypticFastFood.CustomerContext(minor.Id, minor.Age, minor.VisitCount, minor.MembershipLevel, minor.HasParentApproval));
        }
        catch (Exception ex)
        {
            Console.WriteLine("LSP VIOLATED! " + ex.Message);
        }
    }
}
