namespace ApocalypticFastFood;

public class DiscountManager
{
    public bool AcceptsMarketing;
    public int Age;
    public double AverageSpend;
    public bool CheckedIn;
    public int ComplaintsCount;
    public int ConsecutiveVisits;
    public string? CreditCardType;
    public int CurrentMonth;
    public int CustomerType; // 1=regular, 2=vip, 3=employee, 4=senior, 5=student, 6=minor, 7=banned
    public string Day;
    public int DaysLastVisit;
    public string DeviceType;
    public string DietaryPreference;
    public bool EmailSubscribed;
    public string EmployeeName;
    public int FamilyMembers;
    public string FavoriteItem;
    public bool HasAllergies;
    public bool HasApp;
    public bool HasKids;
    public bool HasLoyaltyCard;
    public int Hour;
    public bool IsBirthday;
    public bool IsCurbside;
    public bool IsDelivery;
    public bool IsDineIn;
    public bool IsDriveThru;
    public bool IsFirstOrder;
    public bool IsHoliday;
    public bool IsRushHour;
    public bool IsWeekend;
    public int ItemCount;
    public Dictionary<string, double> ItemPrices = new();
    public Dictionary<string, int> ItemQuantities = new();
    public List<string> Items = new();
    public double LastTipAmount;
    public double Latitude;
    public bool LeftReview;
    public double Longitude;
    public int ManagerApproval;
    public string MembershipLevel; // "Bronze", "Silver", "Gold", "Platinum", "Diamond", "Unicorn"
    public int Minute;
    public int MonthsSinceMembership;
    public int OrderNumber;
    public List<string> PastOrders = new();
    public string PaymentMethod;
    public string PreviousOrder;
    public string PromoCode;
    public int ReferralCount;
    public string Region;
    public int RestaurantId;
    public int ReviewStars;
    public bool SmsSubscribed;
    public string SocialMediaFollow;
    public int StreakDays;
    public int Temperature;
    public double TotalAmount;
    public int VisitCount;
    public string Weather;

    public double CalculateDiscount()
    {
        double discount = 0;
        var multiplier = 1.0;
        double tempDiscount = 0;
        var bonusPoints = 0;
        var qualifiesForSpecial = false;
        double s1 = 0,
            s2 = 0,
            s3 = 0,
            s4 = 0,
            s5 = 0,
            s6 = 0,
            s7 = 0,
            s8 = 0,
            s9 = 0,
            s10 = 0,
            s11 = 0,
            s12 = 0,
            s13 = 0,
            s14 = 0,
            s15 = 0,
            s16 = 0,
            s17 = 0,
            s18 = 0,
            s19 = 0,
            s20 = 0;

        // Compute promo-related discounts via the new DiscountEngine (PromoCodeRule)
        var promoDiscount = new DiscountEngine(new IDiscountRule[] { new PromoCodeRule() }).Calculate(this);

        ItemPrices["burger"] = 8.99;
        ItemPrices["fries"] = 3.49;
        ItemPrices["shake"] = 4.99;
        ItemPrices["nuggets"] = 6.49;
        ItemPrices["salad"] = 7.99;

        foreach (var item in Items)
        {
            ItemQuantities.TryAdd(item, 0);
            ItemQuantities[item]++;
        }

        if (Items.Contains("burger") && Items.Contains("fries") && Items.Contains("shake"))
        {
            if (ItemQuantities["burger"] >= 2 && ItemQuantities["fries"] >= 2 && ItemQuantities["shake"] >= 2)
                switch (CustomerType)
                {
                    case 2:
                    {
                        switch (MembershipLevel)
                        {
                            case "Diamond":
                            {
                                switch (VisitCount)
                                {
                                    case > 100:
                                    {
                                        if (IsBirthday)
                                        {
                                            if (Hour is >= 14 and <= 16)
                                                s1 = Temperature > 80 ? 50.0 : 45.0;
                                            else
                                                s1 = 40.0;
                                        }
                                        else
                                        {
                                            s1 = 35.0;
                                        }

                                        break;
                                    }
                                    case > 50:
                                        s1 = 30.0;
                                        break;
                                    default:
                                        s1 = 25.0;
                                        break;
                                }

                                break;
                            }
                            case "Platinum":
                            {
                                s1 = TotalAmount > 100 ? 28.0 : 22.0;
                                break;
                            }
                            case "Gold":
                                s1 = 18.0;
                                break;
                            default:
                                s1 = 15.0;
                                break;
                        }

                        break;
                    }
                    case 5:
                    {
                        if (Day is "Tuesday" or "Thursday")
                            s1 = Age < 18 ? 20.0 : 16.0;
                        else
                            s1 = 12.0;

                        break;
                    }
                    case 3:
                        s1 = 45.0;
                        break;
                    default:
                        s1 = 10.0;
                        break;
                }
            else
                s1 = Day == "Wednesday" ? 8.0 : 5.0;
        }

        switch (Hour)
        {
            case >= 14 and <= 16:
            {
                if (Minute is >= 0 and <= 30)
                {
                    if (CustomerType == 4)
                    {
                        if (Age >= 70)
                        {
                            if (HasLoyaltyCard)
                                s2 = VisitCount > 20 ? 30.0 : 25.0;
                            else
                                s2 = 20.0;
                        }
                        else
                        {
                            s2 = 15.0;
                        }
                    }
                    else
                    {
                        s2 = 12.0;
                    }
                }
                else
                {
                    s2 = 8.0;
                }

                break;
            }
            case >= 6 and <= 9:
            {
                if (IsDineIn)
                {
                    if (Items.Contains("burger"))
                        s2 = Day == "Monday" ? 18.0 : 12.0;
                    else
                        s2 = 8.0;
                }
                else if (IsDriveThru)
                {
                    s2 = 6.0;
                }

                break;
            }
        }
        // Time-of-day logic is being migrated to TimeOfDayRule in DiscountEngine.
        var timeDiscount = new DiscountEngine(new IDiscountRule[] { new TimeOfDayRule() }).Calculate(this);
        // Zero out legacy slots covered by the TimeOfDayRule to avoid double-counting
        s2 = 0.0;
        s8 = 0.0;
        if (ItemQuantities.ContainsKey("burger"))
        {
            var bc = ItemQuantities["burger"];
            if (bc >= 2)
                switch (Day)
                {
                    case "Tuesday":
                    {
                        if (Hour is >= 11 and <= 14)
                        {
                            if (bc >= 4)
                            {
                                if (CustomerType == 2)
                                {
                                    if (MembershipLevel is "Platinum" or "Diamond")
                                    {
                                        if (HasApp)
                                        {
                                            if (EmailSubscribed)
                                                s3 = 17.98 * 3;
                                            else
                                                s3 = 17.98 * 2.5;
                                        }
                                        else
                                        {
                                            s3 = 17.98 * 2;
                                        }
                                    }
                                    else
                                    {
                                        s3 = 17.98 * 1.5;
                                    }
                                }
                                else
                                {
                                    s3 = 17.98;
                                }
                            }
                            else
                            {
                                s3 = 8.99;
                            }
                        }
                        else
                        {
                            s3 = 8.99 * 0.8;
                        }

                        break;
                    }
                    case "Friday":
                        s3 = 8.99 * 0.5;
                        break;
                }
        }

        if (ItemQuantities.ContainsKey("shake"))
        {
            var sc = ItemQuantities["shake"];
            if (sc >= 2)
            {
                if (Weather is "hot" or "very hot")
                {
                    if (Temperature > 90)
                        s3 += 4.99 * 1.5;
                    else
                        s3 += 4.99;
                }
                else
                {
                    s3 += 4.99 * 0.5;
                }
            }
        }

        if (FamilyMembers >= 4)
        {
            if (HasKids)
            {
                if (ItemCount >= 8)
                {
                    if (Day is "Saturday" or "Sunday")
                    {
                        if (Hour is >= 12 and <= 14)
                        {
                            if (TotalAmount > 80)
                                s4 = IsHoliday ? 35.0 : 28.0;
                            else
                                s4 = 22.0;
                        }
                        else
                        {
                            s4 = 18.0;
                        }
                    }
                    else
                    {
                        s4 = 15.0;
                    }
                }
                else
                {
                    s4 = 10.0;
                }
            }
            else
            {
                s4 = 8.0;
            }
        }

        // Family/kids discounts are being migrated to FamilyRule in DiscountEngine.
        var familyDiscount = 0.0;
        if (FamilyMembers >= 4)
        {
            familyDiscount = new DiscountEngine(new IDiscountRule[] { new FamilyRule() }).Calculate(this);
            // zero out legacy s4 to avoid double-counting when we migrate this rule
            s4 = 0.0;
        }

        if (VisitCount > 0)
            switch (VisitCount)
            {
                case >= 100:
                {
                    if (CustomerType == 2)
                    {
                        if (MembershipLevel == "Diamond")
                        {
                            if (MonthsSinceMembership > 24)
                            {
                                if (AverageSpend > 75)
                                {
                                    if (ConsecutiveVisits > 10)
                                    {
                                        if (LeftReview && ReviewStars == 5)
                                        {
                                            if (ReferralCount > 10)
                                            {
                                                s5 = 60.0;
                                                multiplier *= 1.5;
                                            }
                                            else
                                            {
                                                s5 = 50.0;
                                                multiplier *= 1.4;
                                            }
                                        }
                                        else
                                        {
                                            s5 = 45.0;
                                            multiplier *= 1.3;
                                        }
                                    }
                                    else
                                    {
                                        s5 = 40.0;
                                        multiplier *= 1.25;
                                    }
                                }
                                else
                                {
                                    s5 = 35.0;
                                    multiplier *= 1.2;
                                }
                            }
                            else
                            {
                                s5 = 30.0;
                            }
                        }
                        else
                        {
                            s5 = 25.0;
                        }
                    }
                    else
                    {
                        s5 = 20.0;
                    }

                    break;
                }
                case >= 50:
                    s5 = 15.0;
                    break;
                case >= 25:
                    s5 = 10.0;
                    break;
                case >= 10:
                    s5 = 6.0;
                    break;
                default:
                    s5 = 3.0;
                    break;
            }

        // Visit-count based discounts are being migrated to VisitCountRule in DiscountEngine.
        // Compute visitDiscount via the engine and zero-out legacy s5 to avoid double-counting.
        var visitDiscount = 0.0;
        if (VisitCount > 0)
        {
            visitDiscount = new DiscountEngine(new IDiscountRule[] { new VisitCountRule() }).Calculate(this);
            s5 = 0.0;
        }

        if (CustomerType == 5)
            switch (Day)
            {
                case "Monday":
                    if (Hour is >= 15 and <= 18)
                        switch (Age)
                        {
                            case < 18:
                                if (HasApp)
                                    s6 = 15.0;
                                else
                                    s6 = 12.0;
                                break;
                            case >= 18 and <= 22:
                                s6 = 10.0;
                                break;
                            case > 22:
                                s6 = 6.0;
                                break;
                        }
                    else
                        s6 = 5.0;

                    break;
                case "Tuesday":
                case "Thursday":
                    s6 = ItemCount >= 3 ? 12.0 : 8.0;
                    break;
                default:
                    s6 = 4.0;
                    break;
            }
        
        if (IsBirthday)
        {
            if (CustomerType == 2)
            {
                if (MembershipLevel == "Diamond")
                {
                    if (VisitCount > 100)
                    {
                        if (TotalAmount > 100)
                        {
                            if (FamilyMembers > 2)
                            {
                                s7 = 70.0;
                                multiplier *= 1.6;
                            }
                            else
                            {
                                s7 = 60.0;
                                multiplier *= 1.5;
                            }
                        }
                        else
                        {
                            s7 = 50.0;
                            multiplier *= 1.4;
                        }
                    }
                    else
                    {
                        s7 = 40.0;
                        multiplier *= 1.3;
                    }
                }
                else
                {
                    s7 = 30.0;
                    multiplier *= 1.2;
                }
            }
            else
            {
                s7 = 25.0;
                multiplier *= 1.15;
            }
        }

        switch (Hour)
        {
            case >= 6 and <= 8:
            {
                if (Minute <= 30)
                {
                    if (IsDineIn)
                    {
                        if (Items.Contains("burger"))
                        {
                            if (Day is "Monday" or "Wednesday" or "Friday")
                            {
                                if (CustomerType is 2 or 4)
                                {
                                    s8 = HasApp ? 22.0 : 18.0;
                                }
                                else
                                {
                                    s8 = 15.0;
                                }
                            }
                            else
                            {
                                s8 = 12.0;
                            }
                        }
                        else
                        {
                            s8 = 10.0;
                        }
                    }
                    else
                    {
                        s8 = 8.0;
                    }
                }
                else
                {
                    s8 = 6.0;
                }

                break;
            }
            case >= 22:
            {
                if (IsDriveThru)
                    s8 = ItemCount >= 3 ? 16.0 : 12.0;
                else
                    s8 = 10.0;

                break;
            }
        }

        switch (Weather)
        {
            case "rainy":
                if (IsDriveThru || IsDelivery)
                {
                    if (TotalAmount > 50)
                    {
                        if (Temperature < 60)
                        {
                            if (Hour is >= 17 and <= 20)
                            {
                                if (Items.Contains("burger"))
                                {
                                    if (CustomerType == 2)
                                    {
                                        s9 = HasApp ? 25.0 : 20.0;
                                    }
                                    else
                                    {
                                        s9 = 15.0;
                                    }
                                }
                                else
                                {
                                    s9 = 12.0;
                                }
                            }
                            else
                            {
                                s9 = 10.0;
                            }
                        }
                        else
                        {
                            s9 = 8.0;
                        }
                    }
                    else
                    {
                        s9 = 6.0;
                    }
                }
                else
                {
                    s9 = 4.0;
                }

                break;
            case "snowy":
                if (Temperature < 32)
                    s9 = IsDineIn ? 20.0 : 15.0;
                else
                    s9 = 10.0;

                break;
            case "stormy":
                s9 = 25.0;
                break;
            case "hot":
                if (Temperature > 90) s9 = Items.Contains("shake") ? 12.0 : 8.0;

                break;
        }

        if (ReferralCount > 0)
            switch (ReferralCount)
            {
                case >= 20:
                {
                    if (CustomerType == 2)
                    {
                        if (MembershipLevel == "Diamond")
                        {
                            if (MonthsSinceMembership > 12)
                                s10 = AverageSpend > 60 ? 45.0 : 38.0;
                            else
                                s10 = 32.0;
                        }
                        else
                        {
                            s10 = 28.0;
                        }
                    }
                    else
                    {
                        s10 = 22.0;
                    }

                    break;
                }
                case >= 10:
                    s10 = 18.0;
                    break;
                case >= 5:
                    s10 = 12.0;
                    break;
                default:
                    s10 = 6.0;
                    break;
            }

        switch (Region)
        {
            case "Northeast":
            {
                if (Latitude is >= 40.7 and <= 40.8)
                {
                    if (Longitude is >= -74.0 and <= -73.9)
                    {
                        if (RestaurantId == 101)
                        {
                            if (Hour is >= 12 and <= 14)
                            {
                                if (IsDineIn)
                                {
                                    s11 = TotalAmount > 100 ? 30.0 : 22.0;
                                }
                                else
                                {
                                    s11 = 18.0;
                                }
                            }
                            else
                            {
                                s11 = 15.0;
                            }
                        }
                        else
                        {
                            s11 = 12.0;
                        }
                    }
                    else
                    {
                        s11 = 8.0;
                    }
                }

                break;
            }
            case "West":
            {
                s11 = Weather == "sunny" ? 10.0 : 6.0;
                break;
            }
        }
        
        if (StreakDays > 0)
            switch (StreakDays)
            {
                case >= 30:
                {
                    if (ConsecutiveVisits >= 20)
                    {
                        if (CustomerType == 2)
                        {
                            if (AverageSpend > 70)
                            {
                                if (HasApp && EmailSubscribed)
                                {
                                    s12 = 55.0;
                                    multiplier *= 1.35;
                                }
                                else
                                {
                                    s12 = 45.0;
                                    multiplier *= 1.25;
                                }
                            }
                            else
                            {
                                s12 = 35.0;
                            }
                        }
                        else
                        {
                            s12 = 28.0;
                        }
                    }
                    else
                    {
                        s12 = 22.0;
                    }

                    break;
                }
                case >= 14:
                    s12 = 15.0;
                    break;
                case >= 7:
                    s12 = 10.0;
                    break;
                default:
                    s12 = 5.0;
                    break;
            }

        if (string.IsNullOrEmpty(PromoCode))
        {
            switch (PromoCode)
            {
                case "SAVE10":
                    if (TotalAmount > 50)
                    {
                        s13 = CustomerType == 2 ? 15.0 : 10.0;
                    }
                    else
                    {
                        s13 = 5.0;
                    }

                    break;
                case "SAVE20":
                    s13 = 20.0;
                    break;
                case "VIP50":
                    if (CustomerType == 2)
                        s13 = MembershipLevel switch
                        {
                            "Diamond" => VisitCount > 100 ? 70.0 : 60.0,
                            "Platinum" => 55.0,
                            "Gold" => 50.0,
                            _ => 45.0
                        };
                    else
                        s13 = 10.0;

                    break;
                case "STUDENT25":
                    if (CustomerType == 5) s13 = Age < 22 ? 25.0 : 15.0;

                    break;
                case "FREEFRIES":
                    if (Items.Contains("fries")) s13 = 3.49 * ItemQuantities["fries"];
                    break;
            }
        }
        else
        {
            // Promo handling migrated to DiscountEngine (promoDiscount)
            s13 = 0.0;
        }
        
        if (SocialMediaFollow is not "" and not null)
            switch (SocialMediaFollow)
            {
                case "instagram":
                    if (LeftReview)
                    {
                        if (ReviewStars == 5)
                        {
                            if (HasApp)
                            {
                                if (EmailSubscribed)
                                {
                                    if (SmsSubscribed)
                                        s14 = ReferralCount > 3 ? 28.0 : 22.0;
                                    else
                                        s14 = 18.0;
                                }
                                else
                                {
                                    s14 = 15.0;
                                }
                            }
                            else
                            {
                                s14 = 12.0;
                            }
                        }
                        else
                        {
                            s14 = 8.0;
                        }
                    }
                    else
                    {
                        s14 = 5.0;
                    }

                    break;
                case "facebook":
                    s14 = 6.0;
                    break;
                case "twitter":
                    s14 = 4.0;
                    break;
            }

        switch (PaymentMethod)
        {
            case "cash":
                s15 = TotalAmount switch
                {
                    > 100 => CustomerType == 2 ? 12.0 : 8.0,
                    > 50 => 5.0,
                    _ => 3.0
                };

                break;
            case "credit":
                if (CreditCardType == "premium")
                {
                    if (CustomerType == 2)
                        s15 = TotalAmount > 150 ? 18.0 : 12.0;
                    else
                        s15 = 8.0;
                }
                else
                {
                    s15 = 5.0;
                }

                break;
            case "app":
                if (HasApp)
                {
                    if (EmailSubscribed && SmsSubscribed)
                        s15 = 15.0;
                    else
                        s15 = 10.0;
                }

                break;
        }

        if (IsFirstOrder)
        {
            if (HasApp)
            {
                if (EmailSubscribed)
                {
                    if (TotalAmount > 50)
                        s16 = ItemCount >= 4 ? 30.0 : 25.0;
                    else
                        s16 = 20.0;
                }
                else
                {
                    s16 = 15.0;
                }
            }
            else
            {
                s16 = 10.0;
            }
        }

        if (IsRushHour)
            switch (Hour)
            {
                case >= 12 and <= 13:
                {
                    if (IsDriveThru)
                    {
                        if (ItemCount >= 5)
                        {
                            if (CustomerType == 2)
                            {
                                if (MembershipLevel == "Diamond")
                                {
                                    if (VisitCount > 100)
                                        s17 = -2.0;
                                    else
                                        s17 = -5.0;
                                }
                                else
                                {
                                    s17 = -8.0;
                                }
                            }
                            else
                            {
                                s17 = -12.0;
                            }
                        }
                        else
                        {
                            s17 = -8.0;
                        }
                    }
                    else
                    {
                        s17 = -5.0;
                    }

                    break;
                }
                case >= 18 and <= 19:
                    s17 = -10.0;
                    break;
            }

        if (DietaryPreference is not "" and not null)
            switch (DietaryPreference)
            {
                case "vegetarian":
                    if (Items.Contains("salad"))
                    {
                        if (ItemQuantities["salad"] >= 2)
                        {
                            if (Day is "Monday" or "Wednesday")
                            {
                                if (CustomerType == 2)
                                {
                                    s18 = HasApp ? 18.0 : 14.0;
                                }
                                else
                                {
                                    s18 = 10.0;
                                }
                            }
                            else
                            {
                                s18 = 8.0;
                            }
                        }
                        else
                        {
                            s18 = 5.0;
                        }
                    }

                    break;
                case "vegan":
                    if (!Items.Contains("burger") && !Items.Contains("nuggets")) s18 = 12.0;
                    break;
                case "gluten-free":
                    s18 = HasAllergies ? 8.0 : 5.0;
                    break;
            }
        
        if (PreviousOrder is not "" and not null)
        {
            var currentOrder = string.Join(",", Items.OrderBy(x => x));
            if (currentOrder == PreviousOrder)
            {
                if (DaysLastVisit <= 7)
                {
                    if (DaysLastVisit <= 3)
                    {
                        if (ConsecutiveVisits >= 5)
                        {
                            if (CustomerType == 2)
                            {
                                if (MembershipLevel is "Diamond" or "Platinum")
                                {
                                    if (AverageSpend > 80)
                                    {
                                        if (HasApp && EmailSubscribed)
                                        {
                                            s19 = 35.0;
                                            multiplier *= 1.25;
                                        }
                                        else
                                        {
                                            s19 = 28.0;
                                        }
                                    }
                                    else
                                    {
                                        s19 = 22.0;
                                    }
                                }
                                else
                                {
                                    s19 = 18.0;
                                }
                            }
                            else
                            {
                                s19 = 15.0;
                            }
                        }
                        else
                        {
                            s19 = 10.0;
                        }
                    }
                    else
                    {
                        s19 = 8.0;
                    }
                }
                else
                {
                    s19 = 5.0;
                }
            }
        }

        if (ManagerApproval > 0)
            switch (ManagerApproval)
            {
                case 1:
                {
                    if (ComplaintsCount > 0)
                    {
                        if (ComplaintsCount >= 3)
                            s20 = CustomerType == 2 ? 40.0 : 30.0;
                        else
                            s20 = 20.0;
                    }
                    else
                    {
                        s20 = 15.0;
                    }

                    break;
                }
                case 2:
                {
                    if (TotalAmount > 200)
                        s20 = ItemCount >= 10 ? 50.0 : 35.0;
                    else
                        s20 = 25.0;

                    break;
                }
            }

        discount = s1 + s2 + s3 + s4 + s5 + s6 + s7 + s8 + s9 + s10 + s11 + s12 + s13 + s14 + s15 + s16 + s17 + s18 +
               s19 + s20;

        // Add promoDiscount, visitDiscount, timeDiscount and familyDiscount computed by the DiscountEngine to avoid duplicating logic
        discount += promoDiscount + (visitDiscount) + timeDiscount + familyDiscount;

        discount *= multiplier;

        if (TotalAmount > 500)
            if (CustomerType == 2)
            {
                if (MembershipLevel == "Diamond")
                {
                    if (VisitCount > 200)
                    {
                        if (AverageSpend > 100)
                        {
                            if (ReferralCount > 15)
                            {
                                if (StreakDays > 60)
                                {
                                    if (IsBirthday)
                                    {
                                        discount += 100.0;
                                    }
                                    else
                                    {
                                        discount += 80.0;
                                    }
                                }
                                else
                                {
                                    discount += 60.0;
                                }
                            }
                            else
                            {
                                discount += 50.0;
                            }
                        }
                        else
                        {
                            discount += 40.0;
                        }
                    }
                    else
                    {
                        discount += 30.0;
                    }
                }
                else
                {
                    discount += 20.0;
                }
            }

        switch (CustomerType)
        {
            case 1:
            {
                if (VisitCount < 5)
                    if (IsFirstOrder)
                        discount += 10.0;
                break;
            }
            case 2:
            {
                switch (MembershipLevel)
                {
                    case "Bronze":
                        discount += 5.0;
                        break;
                    case "Silver":
                    {
                        if (MonthsSinceMembership > 6)
                            discount += 8.0;
                        else
                            discount += 6.0;
                        break;
                    }
                    case "Gold":
                    {
                        if (MonthsSinceMembership > 12)
                        {
                            if (VisitCount > 30)
                                discount += 15.0;
                            else
                                discount += 12.0;
                        }
                        else
                        {
                            discount += 10.0;
                        }

                        break;
                    }
                }

                break;
            }
            case 3:
            {
                if (RestaurantId is 101 or 102) discount += 20.0;
                break;
            }
            case 6:
                throw new Exception("Minors need parent approval!");
            case 7:
                discount = -999999.0;
                return discount;
        }

        if (discount > TotalAmount * 0.95) discount = TotalAmount * 0.95;

        if (ComplaintsCount > 5) discount -= 20.0;

        if (LastTipAmount > 10) discount += 5.0;

        return discount;
    }

    public string GenerateReceipt()
    {
        return new ReceiptFormatter().Format(this);
    }

// Responsibility: format a receipt string for a given DiscountManager state
public class ReceiptFormatter
{
    public string Format(DiscountManager dm)
    {
        var receipt = "====================================\n";
        receipt += "    FAST FOOD MEGA CHAIN\n";
        receipt += "====================================\n";

        // Customer type display - magic numbers!
        receipt += "Customer: ";
        switch (dm.CustomerType)
        {
            case 1: receipt += "Regular\n"; break;
            case 2:
                receipt += "VIP - " + dm.MembershipLevel + "\n";
                if (dm.MembershipLevel == "Diamond") receipt += "*** PREMIUM CUSTOMER ***\n";
                break;
            case 3: receipt += "Employee\n"; break;
            case 4: receipt += "Senior (Age: " + dm.Age + ")\n"; break;
            case 5: receipt += "Student\n"; break;
            case 6: receipt += "Minor - NEEDS APPROVAL\n"; break;
            case 7: receipt += "BANNED CUSTOMER\n"; break;
            default: receipt += "Unknown\n"; break;
        }

        receipt += "Order #: " + dm.OrderNumber + "\n";
        receipt += "Date: " + dm.Day + "\n";
        receipt += "Time: " + dm.Hour + ":" + dm.Minute + "\n";
        receipt += "Location: Restaurant #" + dm.RestaurantId + "\n";

        if (dm.IsDineIn)
            receipt += "Service: Dine-In\n";
        else if (dm.IsDriveThru)
            receipt += "Service: Drive-Thru\n";
        else if (dm.IsCurbside)
            receipt += "Service: Curbside\n";
        else if (dm.IsDelivery)
            receipt += "Service: Delivery\n";

        receipt += "------------------------------------\n";
        receipt += "ITEMS:\n";

        // Display items using price catalog lookup if available, otherwise fallback to common prices
        foreach (var item in dm.ItemQuantities)
        {
            double price = item.Key switch
            {
                "burger" => 8.99,
                "fries" => 3.49,
                "shake" => 4.99,
                "nuggets" => 6.49,
                "salad" => 7.99,
                _ => 0
            };

            // prefer price catalog if present via OrderProcessor usage
            if (dm.ItemPrices != null && dm.ItemPrices.TryGetValue(item.Key, out var p)) price = p;

            receipt += "  " + item.Value + "x " + item.Key.ToUpper() + " @ $" + price + " = $" +
                       (item.Value * price).ToString("F2") + "\n";
        }

        receipt += "------------------------------------\n";
        receipt += "Subtotal: $" + dm.TotalAmount.ToString("F2") + "\n";

        var discount = dm.CalculateDiscount();
        receipt += "Discount: -$" + discount.ToString("F2") + "\n";

        var tax = (dm.TotalAmount - discount) * 0.08;
        receipt += "Tax (8%): $" + tax.ToString("F2") + "\n";

        var total = dm.TotalAmount - discount + tax;
        receipt += "------------------------------------\n";
        receipt += "TOTAL: $" + total.ToString("F2") + "\n";
        receipt += "====================================\n";

        var points = dm.CalculateLoyaltyPoints();
        receipt += "Loyalty Points Earned: " + points + "\n";

        if (dm.IsBirthday) receipt += "\n*** HAPPY BIRTHDAY! ***\n";

        return receipt;
    }
}

    private int CalculateLoyaltyPoints()
    {
        var points = 0;
        
        switch (CustomerType)
        {
            case 1:
                points = (int)(TotalAmount * 1);
                if (HasApp)
                {
                    if (EmailSubscribed)
                    {
                        if (VisitCount > 10)
                            points = (int)(TotalAmount * 2);
                        else
                            points = (int)(TotalAmount * 1.5);
                    }
                    else
                    {
                        points = (int)(TotalAmount * 1.2);
                    }
                }

                break;
            case 2:
                switch (MembershipLevel)
                {
                    case "Bronze":
                        points = (int)(TotalAmount * 2);
                        break;
                    case "Silver":
                        points = (int)(TotalAmount * 2.5);
                        break;
                    case "Gold":
                    {
                        if (VisitCount > 50)
                            points = (int)(TotalAmount * 3.5);
                        else
                            points = (int)(TotalAmount * 3);
                        break;
                    }
                    case "Platinum":
                        points = (int)(TotalAmount * 4);
                        break;
                    case "Diamond":
                    {
                        if (VisitCount > 100)
                            points = (int)(TotalAmount * 6);
                        else
                            points = (int)(TotalAmount * 5);
                        break;
                    }
                }

                break;
            case 3:
                points = (int)(TotalAmount * 1.5);
                break;
            case 5:
                points = (int)(TotalAmount * 1.8);
                break;
        }

        if (IsBirthday) points += 500;

        if (ReferralCount > 0) points += ReferralCount * 50;

        return points;
    }
    
    public void SendEmailReceipt(string email)
    {
        // Hardcoded SMTP logic
        Console.WriteLine("Connecting to smtp.example.com:587");
        Console.WriteLine("Sending to: " + email);
    }

    public void SendSmsReceipt(string phone)
    {
        // Hardcoded SMS API
        Console.WriteLine("Sending SMS to: " + phone);
    }
}

// Price catalog abstractions to centralize item pricing
public interface IPriceCatalog
{
    double GetPrice(string item);
}

public class InMemoryPriceCatalog : IPriceCatalog
{
    private readonly Dictionary<string, double> _prices = new()
    {
        ["burger"] = 8.99,
        ["fries"] = 3.49,
        ["shake"] = 4.99,
        ["nuggets"] = 6.49,
        ["salad"] = 7.99
    };

    public double GetPrice(string item)
    {
        if (item is null) return 0.0;
        return _prices.TryGetValue(item, out var p) ? p : 0.0;
    }
}

public class OrderProcessor
{
    private readonly IOrderRepository _orderRepo;
    private readonly DiscountManager _dm;
    private readonly IEmailSender _emailSender;
    private readonly ISmsSender _smsSender;
    private readonly IPriceCatalog _priceCatalog;
    private readonly IPaymentProcessor _paymentProcessor;

    public OrderProcessor(IPriceCatalog? priceCatalog = null,
                          IOrderRepository? orderRepo = null,
                          DiscountManager? dm = null,
                          IEmailSender? emailSender = null,
                          ISmsSender? smsSender = null,
                          IPaymentProcessor? paymentProcessor = null)
    {
        _priceCatalog = priceCatalog ?? new InMemoryPriceCatalog();
        _orderRepo = orderRepo ?? new DatabaseRepositoryAdapter(new DatabaseService());
        _dm = dm ?? new DiscountManager();
        _emailSender = emailSender ?? new ConsoleEmailSender();
        _smsSender = smsSender ?? new ConsoleSmsSender();
        // Default facade composes basic processors
        _paymentProcessor = paymentProcessor ?? new PaymentProcessorFacade(new CashPaymentProcessor(), new CardPaymentProcessor());
    }
    
    public void ProcessOrder(int custType, string day, int hr, List<string> items)
    {
        _dm.CustomerType = custType;
        _dm.Day = day;
        _dm.Hour = hr;
        _dm.Items = items;

        // Calculate total using price catalog
        _dm.TotalAmount = 0;
        foreach (var item in items)
        {
            _dm.TotalAmount += _priceCatalog.GetPrice(item);
        }

        var disc = _dm.CalculateDiscount();

        Console.WriteLine(_dm.GenerateReceipt());

        // Save order using repository
        _orderRepo.SaveOrder(_dm.OrderNumber, _dm.TotalAmount, disc);

        // Process payment according to selected method (simplified)
        switch (_dm.PaymentMethod)
        {
            case "cash":
                _paymentProcessor.ProcessCash(_dm.TotalAmount);
                break;
            case "credit":
                _paymentProcessor.ProcessCreditCard("4111111111111111", "123", "12/29");
                break;
            case "debit":
                _paymentProcessor.ProcessDebitCard("4111111111111111", "0000");
                break;
            case "paypal":
                _paymentProcessor.ProcessPaypal("customer@paypal", "password");
                break;
            default:
                // unsupported/no-op
                break;
        }

        if (_dm.EmailSubscribed) _emailSender.Send("customer@email.com", "Receipt");
    }
}

public interface IPaymentProcessor
{
    void ProcessCreditCard(string cardNum, string cvv, string exp);
    void ProcessDebitCard(string cardNum, string pin);
    void ProcessPaypal(string email, string password);
    void ProcessCrypto(string wallet, string coin);
    void ProcessGiftCard(string code);
    void ProcessCash(double amount);
    void ProcessCheck(string checkNum);
    void ProcessBankTransfer(string routing, string account);
}

// Segregated payment interfaces (Interface Segregation Principle)
public interface ICashPaymentProcessor
{
    void ProcessCash(double amount);
}

public interface ICardPaymentProcessor
{
    void ProcessCreditCard(string cardNum, string cvv, string exp);
    void ProcessDebitCard(string cardNum, string pin);
}

public interface IPaypalProcessor
{
    void ProcessPaypal(string email, string password);
}

public interface ICryptoProcessor
{
    void ProcessCrypto(string wallet, string coin);
}

public interface IGiftCardProcessor
{
    void ProcessGiftCard(string code);
}

public interface ICheckProcessor
{
    void ProcessCheck(string checkNum);
}

public interface IBankTransferProcessor
{
    void ProcessBankTransfer(string routing, string account);
}

// Backwards-compatible concrete that still implements the legacy interface
public class CashPaymentProcessor : IPaymentProcessor, ICashPaymentProcessor
{
    public void ProcessCash(double amount)
    {
        Console.WriteLine("Processing cash: $" + amount);
    }

    public void ProcessCreditCard(string c, string v, string e)
    {
        throw new NotImplementedException();
    }

    public void ProcessDebitCard(string c, string p)
    {
        throw new NotImplementedException();
    }

    public void ProcessPaypal(string e, string p)
    {
        throw new NotImplementedException();
    }

    public void ProcessCrypto(string w, string c)
    {
        throw new NotImplementedException();
    }

    public void ProcessGiftCard(string c)
    {
        throw new NotImplementedException();
    }

    public void ProcessCheck(string c)
    {
        throw new NotImplementedException();
    }

    public void ProcessBankTransfer(string r, string a)
    {
        throw new NotImplementedException();
    }
}

// Simple card payment processor (stub implementation)
public class CardPaymentProcessor : ICardPaymentProcessor
{
    public void ProcessCreditCard(string cardNum, string cvv, string exp)
    {
        Console.WriteLine($"Processing credit card {cardNum} exp {exp}");
    }

    public void ProcessDebitCard(string cardNum, string pin)
    {
        Console.WriteLine($"Processing debit card {cardNum}");
    }
}

// Facade that composes segregated processors and exposes the legacy large interface
public class PaymentProcessorFacade : IPaymentProcessor
{
    private readonly ICashPaymentProcessor? _cash;
    private readonly ICardPaymentProcessor? _card;
    private readonly IPaypalProcessor? _paypal;
    private readonly ICryptoProcessor? _crypto;
    private readonly IGiftCardProcessor? _gift;
    private readonly ICheckProcessor? _check;
    private readonly IBankTransferProcessor? _bank;

    public PaymentProcessorFacade(ICashPaymentProcessor? cash = null,
                                  ICardPaymentProcessor? card = null,
                                  IPaypalProcessor? paypal = null,
                                  ICryptoProcessor? crypto = null,
                                  IGiftCardProcessor? gift = null,
                                  ICheckProcessor? check = null,
                                  IBankTransferProcessor? bank = null)
    {
        _cash = cash;
        _card = card;
        _paypal = paypal;
        _crypto = crypto;
        _gift = gift;
        _check = check;
        _bank = bank;
    }

    public void ProcessCreditCard(string cardNum, string cvv, string exp)
    {
        if (_card is null) throw new NotSupportedException("Card processing not configured");
        _card.ProcessCreditCard(cardNum, cvv, exp);
    }

    public void ProcessDebitCard(string cardNum, string pin)
    {
        if (_card is null) throw new NotSupportedException("Card processing not configured");
        _card.ProcessDebitCard(cardNum, pin);
    }

    public void ProcessPaypal(string email, string password)
    {
        if (_paypal is null) throw new NotSupportedException("Paypal not configured");
        _paypal.ProcessPaypal(email, password);
    }

    public void ProcessCrypto(string wallet, string coin)
    {
        if (_crypto is null) throw new NotSupportedException("Crypto not configured");
        _crypto.ProcessCrypto(wallet, coin);
    }

    public void ProcessGiftCard(string code)
    {
        if (_gift is null) throw new NotSupportedException("Gift card processing not configured");
        _gift.ProcessGiftCard(code);
    }

    public void ProcessCash(double amount)
    {
        if (_cash is null) throw new NotSupportedException("Cash processing not configured");
        _cash.ProcessCash(amount);
    }

    public void ProcessCheck(string checkNum)
    {
        if (_check is null) throw new NotSupportedException("Check processing not configured");
        _check.ProcessCheck(checkNum);
    }

    public void ProcessBankTransfer(string routing, string account)
    {
        if (_bank is null) throw new NotSupportedException("Bank transfer not configured");
        _bank.ProcessBankTransfer(routing, account);
    }
}

public class EmailService
{
    public void Send(string to, string subject)
    {
        Console.WriteLine("Email sent to " + to);
    }
}

public class SmsService
{
    public void Send(string phone)
    {
        Console.WriteLine("SMS sent to " + phone);
    }
}

// Abstractions for external integrations (Dependency Inversion)
public interface IEmailSender
{
    void Send(string to, string subject, string body = "");
}

public interface ISmsSender
{
    void Send(string phone, string message = "");
}

public interface IOrderRepository
{
    void SaveOrder(int orderId, double total, double discount);
}

// Console adapters to preserve current dev behavior
public class ConsoleEmailSender : IEmailSender
{
    public void Send(string to, string subject, string body = "")
    {
        Console.WriteLine($"Connecting to smtp.example.com:587");
        Console.WriteLine($"Sending to: {to} - {subject}");
    }
}

public class ConsoleSmsSender : ISmsSender
{
    public void Send(string phone, string message = "")
    {
        Console.WriteLine($"Sending SMS to: {phone} - {message}");
    }
}

public class DatabaseRepositoryAdapter : IOrderRepository
{
    private readonly DatabaseService _db;
    public DatabaseRepositoryAdapter(DatabaseService db) => _db = db;
    public void SaveOrder(int orderId, double total, double discount) => _db.SaveOrder(orderId, total, discount);
}

public class DatabaseService
{
    public void SaveOrder(int orderId, double total, double disc)
    {
        Console.WriteLine("Saving order " + orderId + " to database");
    }
}