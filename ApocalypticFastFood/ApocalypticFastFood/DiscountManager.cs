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
        var receipt = "====================================\n";
        receipt += "    FAST FOOD MEGA CHAIN\n";
        receipt += "====================================\n";

        // Customer type display - magic numbers!
        receipt += "Customer: ";
        switch (CustomerType)
        {
            case 1: receipt += "Regular\n"; break;
            case 2:
                receipt += "VIP - " + MembershipLevel + "\n";
                if (MembershipLevel == "Diamond") receipt += "*** PREMIUM CUSTOMER ***\n";
                break;
            case 3: receipt += "Employee\n"; break;
            case 4: receipt += "Senior (Age: " + Age + ")\n"; break;
            case 5: receipt += "Student\n"; break;
            case 6: receipt += "Minor - NEEDS APPROVAL\n"; break;
            case 7: receipt += "BANNED CUSTOMER\n"; break;
            default: receipt += "Unknown\n"; break;
        }

        receipt += "Order #: " + OrderNumber + "\n";
        receipt += "Date: " + Day + "\n";
        receipt += "Time: " + Hour + ":" + Minute + "\n";
        receipt += "Location: Restaurant #" + RestaurantId + "\n";

        if (IsDineIn)
            receipt += "Service: Dine-In\n";
        else if (IsDriveThru)
            receipt += "Service: Drive-Thru\n";
        else if (IsCurbside)
            receipt += "Service: Curbside\n";
        else if (IsDelivery)
            receipt += "Service: Delivery\n";

        receipt += "------------------------------------\n";
        receipt += "ITEMS:\n";

        // Display items with hardcoded prices
        foreach (var item in ItemQuantities)
        {
            var price = item.Key switch
            {
                "burger" => 8.99,
                "fries" => 3.49,
                "shake" => 4.99,
                "nuggets" => 6.49,
                "salad" => 7.99,
                _ => 0
            };

            receipt += "  " + item.Value + "x " + item.Key.ToUpper() + " @ $" + price + " = $" +
                       (item.Value * price).ToString("F2") + "\n";
        }

        receipt += "------------------------------------\n";
        receipt += "Subtotal: $" + TotalAmount.ToString("F2") + "\n";

        var discount = CalculateDiscount();
        receipt += "Discount: -$" + discount.ToString("F2") + "\n";
        
        var tax = (TotalAmount - discount) * 0.08;
        receipt += "Tax (8%): $" + tax.ToString("F2") + "\n";

        var total = TotalAmount - discount + tax;
        receipt += "------------------------------------\n";
        receipt += "TOTAL: $" + total.ToString("F2") + "\n";
        receipt += "====================================\n";
        
        var points = CalculateLoyaltyPoints();
        receipt += "Loyalty Points Earned: " + points + "\n";

        if (IsBirthday) receipt += "\n*** HAPPY BIRTHDAY! ***\n";

        return receipt;
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

public class OrderProcessor
{
    public DatabaseService DbSvc = new();
    public DiscountManager Dm = new();
    public EmailService EmailSvc = new();
    public SmsService SmsSvc = new();
    
    public void ProcessOrder(int custType, string day, int hr, List<string> items)
    {
        Dm.CustomerType = custType;
        Dm.Day = day;
        Dm.Hour = hr;
        Dm.Items = items;

        // Calculate total with magic numbers - violates DRY!
        Dm.TotalAmount = 0;
        foreach (var item in items)
            if (item == "burger")
            {
                Dm.TotalAmount += 8.99;
            }
            else if (item == "fries")
                Dm.TotalAmount += 3.49;
            else if (item == "shake")
                Dm.TotalAmount += 4.99;
            else if (item == "nuggets")
                Dm.TotalAmount += 6.49;
            else if (item == "salad")
                Dm.TotalAmount += 7.99;

        var disc = Dm.CalculateDiscount();

        Console.WriteLine(Dm.GenerateReceipt());

        // Hardcoded database save
        DbSvc.SaveOrder(Dm.OrderNumber, Dm.TotalAmount, disc);

        if (Dm.EmailSubscribed) EmailSvc.Send("customer@email.com", "Receipt");
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

public class CashPaymentProcessor : IPaymentProcessor
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

public class DatabaseService
{
    public void SaveOrder(int orderId, double total, double disc)
    {
        Console.WriteLine("Saving order " + orderId + " to database");
    }
}