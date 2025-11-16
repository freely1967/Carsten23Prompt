namespace ApocalypticFastFood;

public class PromoCodeRule : IDiscountRule
{
    public bool IsApplicable(DiscountManager ctx)
    {
        return !string.IsNullOrEmpty(ctx.PromoCode);
    }

    public decimal Calculate(DiscountManager ctx)
    {
        switch (ctx.PromoCode)
        {
            case "SAVE10":
                return ctx.TotalAmount > 50m ? (ctx.CustomerType == 2 ? 15.0m : 10.0m) : 5.0m;
            case "SAVE20":
                return 20.0m;
            case "VIP50":
                if (ctx.CustomerType == 2)
                {
                    return ctx.MembershipLevel switch
                    {
                        "Diamond" => ctx.VisitCount > 100 ? 70.0m : 60.0m,
                        "Platinum" => 55.0m,
                        "Gold" => 50.0m,
                        _ => 45.0m
                    };
                }

                return 10.0m;
            case "STUDENT25":
                return ctx.CustomerType == 5 ? (ctx.Age < 22 ? 25.0m : 15.0m) : 0.0m;
            case "FREEFRIES":
                return ctx.ItemQuantities != null && ctx.ItemQuantities.ContainsKey("fries") ? 3.49m * ctx.ItemQuantities["fries"] : 0.0m;
            default:
                return 0.0m;
        }
    }
}


public class VisitCountRule : IDiscountRule
{
    public bool IsApplicable(DiscountManager ctx)
    {
        return ctx.VisitCount > 0;
    }

    public decimal Calculate(DiscountManager ctx)
    {
        // Simplified tiered visit-count logic extracted into small helpers for readability
        if (ctx.VisitCount >= 100) return CalculateHighVisitDiscount(ctx);
        if (ctx.VisitCount >= 50) return 15.0m;
        if (ctx.VisitCount >= 25) return 10.0m;
        if (ctx.VisitCount >= 10) return 6.0m;
        return 3.0m;
    }

    private decimal CalculateHighVisitDiscount(DiscountManager ctx)
    {
        if (ctx.CustomerType != 2) return 20.0m;
        if (ctx.MembershipLevel != "Diamond") return 25.0m;
        if (ctx.MonthsSinceMembership <= 24) return 30.0m;
        if (ctx.AverageSpend <= 75m) return 35.0m;
        if (ctx.ConsecutiveVisits <= 10) return 40.0m;
        if (ctx.LeftReview && ctx.ReviewStars == 5)
            return ctx.ReferralCount > 10 ? 60.0m : 45.0m;
        return 45.0m;
    }
}

// Multiplier-aware visit-count rule (V2): returns multiplier adjustments where applicable
public class VisitCountRuleV2 : IDiscountRuleV2
{
    public bool IsApplicable(DiscountManager ctx)
    {
        return ctx.VisitCount > 0;
    }

    public DiscountResult CalculateResult(DiscountManager ctx)
    {
        // Multiplier is a pure numeric factor (double) — not a monetary value.
        // When applying to money, cast explicitly to decimal: `money * (decimal)multiplier`.
        double multiplier = 1.0;
        decimal discount = 0.0m; // visit-discount handled by VisitCountRule (legacy)

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
                                        multiplier *= 1.5;
                                    }
                                    else
                                    {
                                        multiplier *= 1.4;
                                    }
                                }
                                else
                                {
                                    multiplier *= 1.3;
                                }
                            }
                            else
                            {
                                multiplier *= 1.25;
                            }
                        }
                        else
                        {
                            multiplier *= 1.2;
                        }
                    }
                }
            }
        }

        return new DiscountResult(discount, multiplier);
    }
}

public class TimeOfDayRule : IDiscountRule
{
    public bool IsApplicable(DiscountManager ctx)
    {
        // Time-based rules apply when Hour is set (non-negative assumed)
        return true;
    }

    public decimal Calculate(DiscountManager ctx)
    {
        decimal total = 0.0m;

        // Hour-based primary windows (mutually exclusive)
        if (ctx.Hour >= 14 && ctx.Hour <= 16)
        {
            total += AfternoonWindowDiscount(ctx);
        }
        else if (ctx.Hour >= 6 && ctx.Hour <= 9)
        {
            total += MorningWindowDiscount(ctx);
        }
        else if (ctx.Hour >= 22)
        {
            total += LateNightWindowDiscount(ctx);
        }

        // Rush-hour penalties apply in addition to primary windows
        total += RushHourPenalty(ctx);

        return total;
    }

    private decimal AfternoonWindowDiscount(DiscountManager ctx)
    {
        // Afternoon window: 14:00-16:59 with finer minute split
        if (ctx.Minute >= 0 && ctx.Minute <= 30)
        {
            if (ctx.CustomerType == 4)
            {
                if (ctx.Age >= 70)
                {
                    if (ctx.HasLoyaltyCard) return ctx.VisitCount > 20 ? 30.0m : 25.0m;
                    return 20.0m;
                }

                return 15.0m;
            }

            return 12.0m;
        }

        return 8.0m;
    }

    private decimal MorningWindowDiscount(DiscountManager ctx)
    {
        // Morning window: 6:00-9:59
        if (ctx.IsDineIn)
        {
            return ctx.Items.Contains("burger") ? (ctx.Day == "Monday" ? 18.0m : 12.0m) : 8.0m;
        }

        if (ctx.IsDriveThru) return 6.0m;
        return 0.0m;
    }

    private decimal LateNightWindowDiscount(DiscountManager ctx)
    {
        // Late-night window: 22:00+
        if (ctx.IsDriveThru) return ctx.ItemCount >= 3 ? 16.0m : 12.0m;
        return 10.0m;
    }

    private decimal RushHourPenalty(DiscountManager ctx)
    {
        if (!ctx.IsRushHour) return 0.0m;

        // Midday penalty (12:00-13:59)
        if (ctx.Hour >= 12 && ctx.Hour <= 13)
        {
            if (ctx.IsDriveThru)
            {
                if (ctx.ItemCount >= 5)
                {
                    if (ctx.CustomerType == 2)
                    {
                        if (ctx.MembershipLevel == "Diamond")
                        {
                            return ctx.VisitCount > 100 ? -2.0m : -5.0m;
                        }

                        return -8.0m;
                    }

                    return -12.0m;
                }

                return -8.0m;
            }

            return -5.0m;
        }

        // Evening penalty (18:00-19:59)
        if (ctx.Hour >= 18 && ctx.Hour <= 19) return -10.0m;

        return 0.0m;
    }
}

public class FamilyRule : IDiscountRule
{
    public bool IsApplicable(DiscountManager ctx)
    {
        return ctx.FamilyMembers >= 4;
    }

    public decimal Calculate(DiscountManager ctx)
    {
        if (ctx.FamilyMembers < 4) return 0.0m;

        // Replicate the s4 logic from the monolith for family-based discounts
        if (ctx.HasKids)
        {
            if (ctx.ItemCount >= 8)
            {
                if (ctx.Day is "Saturday" or "Sunday")
                {
                    if (ctx.Hour is >= 12 and <= 14)
                    {
                        if (ctx.TotalAmount > 80m)
                            return ctx.IsHoliday ? 35.0m : 28.0m;
                        return 22.0m;
                    }
                    return 18.0m;
                }

                return 15.0m;
            }

            return 10.0m;
        }

        return 8.0m;
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
        // Clear, tiered birthday rule using early returns for readability
        if (ctx.CustomerType != 2)
            return new DiscountResult(25.0m, 1.15);

        // VIP path
        if (ctx.MembershipLevel != "Diamond")
            return new DiscountResult(30.0m, 1.2);

        // Diamond VIP
        if (ctx.VisitCount <= 100)
            return new DiscountResult(40.0m, 1.3);

        // VisitCount > 100
        if (ctx.TotalAmount <= 100)
            return new DiscountResult(50.0m, 1.4);

        // TotalAmount > 100
        if (ctx.FamilyMembers > 2)
            return new DiscountResult(70.0m, 1.6);

        return new DiscountResult(60.0m, 1.5);
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
        // Flattened streak rule
        if (ctx.StreakDays < 7) return new DiscountResult(5.0m, 1.0);
        if (ctx.StreakDays < 14) return new DiscountResult(10.0m, 1.0);
        if (ctx.StreakDays < 30) return new DiscountResult(15.0m, 1.0);

        // ctx.StreakDays >= 30
        if (ctx.ConsecutiveVisits < 20) return new DiscountResult(22.0m, 1.0);

        if (ctx.CustomerType != 2) return new DiscountResult(28.0m, 1.0);

        // VIP path
        if (ctx.AverageSpend <= 70) return new DiscountResult(35.0m, 1.0);

        // AverageSpend > 70
        if (ctx.HasApp && ctx.EmailSubscribed) return new DiscountResult(55.0m, 1.35);

        return new DiscountResult(45.0m, 1.25);
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
        // Flattened previous-order discount logic with early returns and small helpers
        var currentOrder = string.Join(",", ctx.Items.OrderBy(x => x));
        if (currentOrder != ctx.PreviousOrder) return new DiscountResult(0.0m, 1.0);

        if (ctx.DaysLastVisit > 7)
        {
            return new DiscountResult(5.0m, 1.0);
        }

        if (ctx.DaysLastVisit > 3)
        {
            return new DiscountResult(8.0m, 1.0);
        }

        // DaysLastVisit <= 3
        if (ctx.ConsecutiveVisits < 5) return new DiscountResult(10.0m, 1.0);

        // ConsecutiveVisits >= 5
        if (ctx.CustomerType != 2) return new DiscountResult(15.0m, 1.0);

        // VIP customer path
        if (ctx.MembershipLevel != "Diamond" && ctx.MembershipLevel != "Platinum")
        {
            return new DiscountResult(18.0m, 1.0);
        }

        // Diamond or Platinum
        if (ctx.AverageSpend <= 80) return new DiscountResult(22.0m, 1.0);

        // AverageSpend > 80
        if (ctx.HasApp && ctx.EmailSubscribed)
        {
            return new DiscountResult(35.0m, 1.25);
        }

        return new DiscountResult(28.0m, 1.0);
    }
}

// Multiplier-aware referral rule: migrates legacy s10 referral logic into a rule
public class ReferralRuleV2 : IDiscountRuleV2
{
    public bool IsApplicable(DiscountManager ctx)
    {
        return ctx.ReferralCount > 0;
    }

    public DiscountResult CalculateResult(DiscountManager ctx)
    {
        // Simplify referral tiers with clear boundaries and early returns
        if (ctx.ReferralCount >= 20)
        {
            if (ctx.CustomerType != 2) return new DiscountResult(22.0m, 1.0);
            if (ctx.MembershipLevel != "Diamond") return new DiscountResult(28.0m, 1.0);
            if (ctx.MonthsSinceMembership > 12) return new DiscountResult(ctx.AverageSpend > 60 ? 45.0m : 38.0m, 1.0);
            return new DiscountResult(32.0m, 1.0);
        }

        if (ctx.ReferralCount >= 10) return new DiscountResult(18.0m, 1.0);
        if (ctx.ReferralCount >= 5) return new DiscountResult(12.0m, 1.0);
        return new DiscountResult(6.0m, 1.0);
    }
}
