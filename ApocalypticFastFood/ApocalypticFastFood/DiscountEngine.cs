namespace ApocalypticFastFood;

using System.Collections.Generic;
using System.Linq;

// DiscountResult: Discount is monetary (decimal). Multiplier is a non-monetary factor (double).
// Keep multiplier as double for fine-grained fractional multipliers; when applying it to
// monetary values cast explicitly: e.g. `amount * (decimal)multiplier`.
public readonly record struct DiscountResult(decimal Discount, double Multiplier);

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
                decimal discount = 0.0m;
                // multiplier is intentionally double (non-monetary). When combining with
                // decimal monetary values perform an explicit cast to avoid compiler errors.
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
