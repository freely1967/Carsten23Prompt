namespace ApocalypticFastFood;
        // Birthday logic migrated to BirthdayRule in DiscountEngineV2.
        var birthdayResult = new DiscountEngineV2(new IDiscountRuleV2[] { new BirthdayRule() }).Calculate(this);
        // zero out legacy s7 to avoid double-counting and apply engine results
        // Streak-based discounts migrated to StreakRule via DiscountEngineV2
        var streakResult = new DiscountEngineV2(new IDiscountRuleV2[] { new StreakRule() }).Calculate(this);
        s12 = 0.0; // zero legacy slot
        discount += streakResult.Discount;
        multiplier *= streakResult.Multiplier;
public class DiscountEngineV2
