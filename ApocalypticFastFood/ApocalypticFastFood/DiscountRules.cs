namespace ApocalypticFastFood;

public class PromoCodeRule : IDiscountRule
{
    public bool IsApplicable(DiscountManager ctx)
    {
        return !string.IsNullOrEmpty(ctx.PromoCode);
    }

    public double Calculate(DiscountManager ctx)
    {
        switch (ctx.PromoCode)
        {
            case "SAVE10":
                return ctx.TotalAmount > 50 ? (ctx.CustomerType == 2 ? 15.0 : 10.0) : 5.0;
            case "SAVE20":
                return 20.0;
            case "VIP50":
                if (ctx.CustomerType == 2)
                {
                    return ctx.MembershipLevel switch
                    {
                        "Diamond" => ctx.VisitCount > 100 ? 70.0 : 60.0,
                        "Platinum" => 55.0,
                        "Gold" => 50.0,
                        _ => 45.0
                    };
                }

                return 10.0;
            case "STUDENT25":
                return ctx.CustomerType == 5 ? (ctx.Age < 22 ? 25.0 : 15.0) : 0.0;
            case "FREEFRIES":
                return ctx.ItemQuantities != null && ctx.ItemQuantities.ContainsKey("fries") ? 3.49 * ctx.ItemQuantities["fries"] : 0.0;
            default:
                return 0.0;
        }
    }
}

public class VisitCountRule : IDiscountRule
{
    public bool IsApplicable(DiscountManager ctx)
    {
        return ctx.VisitCount > 0;
    }

    public double Calculate(DiscountManager ctx)
    {
        if (ctx.VisitCount >= 100)
        {
            if (ctx.CustomerType == 2)
            {
                if (ctx.MembershipLevel == "Diamond")
                {
                    if (ctx.MonthsSinceMembership > 24)
                    {
                        if (ctx.AverageSpend > 75)
                        {
                            if (ctx.ConsecutiveVisits > 10)
                            {
                                if (ctx.LeftReview && ctx.ReviewStars == 5)
                                {
                                    if (ctx.ReferralCount > 10)
                                    {
                                        return 60.0;
                                    }
                                    return 50.0;
                                }
                                return 45.0;
                            }
                            return 40.0;
                        }
                        return 35.0;
                    }
                    return 30.0;
                }
                return 25.0;
            }

            return 20.0;
        }
        else if (ctx.VisitCount >= 50) return 15.0;
        else if (ctx.VisitCount >= 25) return 10.0;
        else if (ctx.VisitCount >= 10) return 6.0;
        else return 3.0;
    }
}

public class TimeOfDayRule : IDiscountRule
{
    public bool IsApplicable(DiscountManager ctx)
    {
        // Time-based rules apply when Hour is set (non-negative assumed)
        return true;
    }

    public double Calculate(DiscountManager ctx)
    {
        double total = 0.0;

        // s2 logic (afternoon & morning windows)
        switch (ctx.Hour)
        {
            case >= 14 and <= 16:
            {
                if (ctx.Minute is >= 0 and <= 30)
                {
                    if (ctx.CustomerType == 4)
                    {
                        if (ctx.Age >= 70)
                        {
                            if (ctx.HasLoyaltyCard)
                                total += ctx.VisitCount > 20 ? 30.0 : 25.0;
                            else
                                total += 20.0;
                        }
                        else
                        {
                            total += 15.0;
                        }
                    }
                    else
                    {
                        total += 12.0;
                    }
                }
                else
                {
                    total += 8.0;
                }

                break;
            }
            case >= 6 and <= 9:
            {
                if (ctx.IsDineIn)
                {
                    if (ctx.Items.Contains("burger"))
                        total += ctx.Day == "Monday" ? 18.0 : 12.0;
                    else
                        total += 8.0;
                }
                else if (ctx.IsDriveThru)
                {
                    total += 6.0;
                }

                break;
            }
            case >= 22:
            {
                if (ctx.IsDriveThru)
                    total += ctx.ItemCount >= 3 ? 16.0 : 12.0;
                else
                    total += 10.0;

                break;
            }
        }

        // s8 morning/early logic overlaps with the above and is handled there; s17 (rush hour penalties) handled below
        if (ctx.IsRushHour)
        {
            switch (ctx.Hour)
            {
                case >= 12 and <= 13:
                {
                    if (ctx.IsDriveThru)
                    {
                        if (ctx.ItemCount >= 5)
                        {
                            if (ctx.CustomerType == 2)
                            {
                                if (ctx.MembershipLevel == "Diamond")
                                {
                                    if (ctx.VisitCount > 100)
                                        total += -2.0;
                                    else
                                        total += -5.0;
                                }
                                else
                                {
                                    total += -8.0;
                                }
                            }
                            else
                            {
                                total += -12.0;
                            }
                        }
                        else
                        {
                            total += -8.0;
                        }
                    }
                    else
                    {
                        total += -5.0;
                    }

                    break;
                }
                case >= 18 and <= 19:
                    total += -10.0;
                    break;
            }
        }

        return total;
    }
}

public class FamilyRule : IDiscountRule
{
    public bool IsApplicable(DiscountManager ctx)
    {
        return ctx.FamilyMembers >= 4;
    }

    public double Calculate(DiscountManager ctx)
    {
        if (ctx.FamilyMembers < 4) return 0.0;

        // Replicate the s4 logic from the monolith for family-based discounts
        if (ctx.HasKids)
        {
            if (ctx.ItemCount >= 8)
            {
                if (ctx.Day is "Saturday" or "Sunday")
                {
                    if (ctx.Hour is >= 12 and <= 14)
                    {
                        if (ctx.TotalAmount > 80)
                            return ctx.IsHoliday ? 35.0 : 28.0;
                        return 22.0;
                    }
                    return 18.0;
                }

                return 15.0;
            }

            return 10.0;
        }

        return 8.0;
    }
}

// Multiplier-aware birthday rule: returns both a discount and a multiplier
public class BirthdayRule : IDiscountRuleV2
{
    public bool IsApplicable(DiscountManager ctx)
    {
        return ctx.IsBirthday;
    }

    public DiscountResult CalculateResult(DiscountManager ctx)
    {
        double discount = 0.0;
        double multiplier = 1.0;

        if (ctx.CustomerType == 2)
        {
            if (ctx.MembershipLevel == "Diamond")
            {
                if (ctx.VisitCount > 100)
                {
                    if (ctx.TotalAmount > 100)
                    {
                        if (ctx.FamilyMembers > 2)
                        {
                            discount = 70.0;
                            multiplier *= 1.6;
                        }
                        else
                        {
                            discount = 60.0;
                            multiplier *= 1.5;
                        }
                    }
                    else
                    {
                        discount = 50.0;
                        multiplier *= 1.4;
                    }
                }
                else
                {
                    discount = 40.0;
                    multiplier *= 1.3;
                }
            }
            else
            {
                discount = 30.0;
                multiplier *= 1.2;
            }
        }
        else
        {
            discount = 25.0;
            multiplier *= 1.15;
        }

        return new DiscountResult(discount, multiplier);
    }
}

public class StreakRule : IDiscountRuleV2
{
    public bool IsApplicable(DiscountManager ctx)
    {
        return ctx.StreakDays > 0;
    }

    public DiscountResult CalculateResult(DiscountManager ctx)
    {
        double discount = 0.0;
        double multiplier = 1.0;

        if (ctx.StreakDays >= 30)
        {
            if (ctx.ConsecutiveVisits >= 20)
            {
                if (ctx.CustomerType == 2)
                {
                    if (ctx.AverageSpend > 70)
                    {
                        if (ctx.HasApp && ctx.EmailSubscribed)
                        {
                            discount = 55.0;
                            multiplier *= 1.35;
                        }
                        else
                        {
                            discount = 45.0;
                            multiplier *= 1.25;
                        }
                    }
                    else
                    {
                        discount = 35.0;
                    }
                }
                else
                {
                    discount = 28.0;
                }
            }
            else
            {
                discount = 22.0;
            }
        }
        else if (ctx.StreakDays >= 14)
        {
            discount = 15.0;
        }
        else if (ctx.StreakDays >= 7)
        {
            discount = 10.0;
        }
        else
        {
            discount = 5.0;
        }

        return new DiscountResult(discount, multiplier);
    }
}

public class PreviousOrderRule : IDiscountRuleV2
{
    public bool IsApplicable(DiscountManager ctx)
    {
        if (string.IsNullOrEmpty(ctx.PreviousOrder)) return false;
        var currentOrder = string.Join(",", ctx.Items.OrderBy(x => x));
        return currentOrder == ctx.PreviousOrder;
    }

    public DiscountResult CalculateResult(DiscountManager ctx)
    {
        double discount = 0.0;
        double multiplier = 1.0;

        var currentOrder = string.Join(",", ctx.Items.OrderBy(x => x));
        if (currentOrder != ctx.PreviousOrder) return new DiscountResult(0.0, 1.0);

        if (ctx.DaysLastVisit <= 7)
        {
            if (ctx.DaysLastVisit <= 3)
            {
                if (ctx.ConsecutiveVisits >= 5)
                {
                    if (ctx.CustomerType == 2)
                    {
                        if (ctx.MembershipLevel is "Diamond" or "Platinum")
                        {
                            if (ctx.AverageSpend > 80)
                            {
                                if (ctx.HasApp && ctx.EmailSubscribed)
                                {
                                    discount = 35.0;
                                    multiplier *= 1.25;
                                }
                                else
                                {
                                    discount = 28.0;
                                }
                            }
                            else
                            {
                                discount = 22.0;
                            }
                        }
                        else
                        {
                            discount = 18.0;
                        }
                    }
                    else
                    {
                        discount = 15.0;
                    }
                }
                else
                {
                    discount = 10.0;
                }
            }
            else
            {
                discount = 8.0;
            }
        }
        else
        {
            discount = 5.0;
        }

        return new DiscountResult(discount, multiplier);
    }
}
