namespace ApocalypticFastFood;

public interface IDiscountRule
{
    bool IsApplicable(DiscountManager ctx);
    double Calculate(DiscountManager ctx);
}

public class DiscountEngine
{
    private readonly IEnumerable<IDiscountRule> _rules;

    public DiscountEngine(IEnumerable<IDiscountRule> rules)
    {
        _rules = rules ?? Enumerable.Empty<IDiscountRule>();
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
