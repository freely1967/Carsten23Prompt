namespace ApocalypticFastFood;

using System;
using System.Linq;

// Backwards-compatible legacy DiscountEngine wrapper for rules implementing IDiscountRule
public class DiscountEngine
{
    private readonly IDiscountRule[] _rules;

    public DiscountEngine(IDiscountRule[] rules)
    {
        _rules = rules ?? Array.Empty<IDiscountRule>();
    }

    public DiscountEngine(System.Collections.Generic.IEnumerable<IDiscountRule> rules)
    {
        _rules = rules?.ToArray() ?? Array.Empty<IDiscountRule>();
    }

    public double Calculate(DiscountManager ctx)
    {
        double total = 0.0;
        foreach (var r in _rules)
        {
            if (r.IsApplicable(ctx)) total += r.Calculate(ctx);
        }

        return total;
    }
}
