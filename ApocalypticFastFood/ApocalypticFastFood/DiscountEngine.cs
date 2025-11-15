namespace ApocalypticFastFood;
        // Birthday logic migrated to BirthdayRule in DiscountEngineV2.
        var birthdayResult = new DiscountEngineV2(new IDiscountRuleV2[] { new BirthdayRule() }).Calculate(this);
        // zero out legacy s7 to avoid double-counting and apply engine results
        s7 = 0.0;
        discount += birthdayResult.Discount;
        multiplier *= birthdayResult.Multiplier;
}

public class DiscountEngineV2
{
    private readonly IEnumerable<IDiscountRuleV2> _rules;

    public DiscountEngineV2(IEnumerable<IDiscountRuleV2> rules)
    {
        _rules = rules ?? Enumerable.Empty<IDiscountRuleV2>();
    }

    public DiscountResult Calculate(DiscountManager ctx)
    {
        double total = 0.0;
        double mult = 1.0;

        foreach (var r in _rules)
        {
            if (!r.IsApplicable(ctx)) continue;
            var res = r.CalculateResult(ctx);
            total += res.Discount;
            // combine multipliers multiplicatively
            mult *= res.Multiplier;
        }

        return new DiscountResult(total, mult);
    }
}
