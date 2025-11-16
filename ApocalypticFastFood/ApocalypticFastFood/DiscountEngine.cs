namespace ApocalypticFastFood;

public readonly record struct DiscountResult(double Discount, double Multiplier);

public interface IDiscountRuleV2
{
    bool IsApplicable(DiscountManager ctx);
    DiscountResult CalculateResult(DiscountManager ctx);
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
        double discount = 0.0;
        double multiplier = 1.0;

        namespace ApocalypticFastFood;

        public readonly record struct DiscountResult(double Discount, double Multiplier);

        public interface IDiscountRuleV2
        {
                bool IsApplicable(DiscountManager ctx);
                DiscountResult CalculateResult(DiscountManager ctx);
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
                        double discount = 0.0;
                        double multiplier = 1.0;

                        foreach (var rule in _rules)
                        {
                                if (rule.IsApplicable(ctx))
                                {
                                        var r = rule.CalculateResult(ctx);
                                        discount += r.Discount;
                                        multiplier *= r.Multiplier;
                                }
                        }

                        return new DiscountResult(discount, multiplier);
                }
        }
                foreach (var rule in _rules)
