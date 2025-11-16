namespace ApocalypticFastFood;

// CustomerContext: lightweight record used across services
public record CustomerContext(int Id = 0, int Age = 0, int VisitCount = 0, string MembershipLevel = "", bool HasParentApproval = false);

// Final conversion: Customer is a plain data holder. Behavioral logic moved to services/adapters.
public class Customer
{
    public int Id { get; set; }
    public int Age { get; set; }
    public int VisitCount { get; set; }
    public string MembershipLevel { get; set; } = string.Empty;
    public bool HasParentApproval { get; set; }

    // Compatibility constructor: accepts legacy policy/strategy params but keeps Customer as a data holder.
    // These parameters are ignored and kept for binary/constructor compatibility with older callers.
    public Customer(IDiscountStrategy? discountStrategy = null,
                    IAlcoholPolicy? alcoholPolicy = null,
                    IPurchaseApprovalPolicy? purchaseApprovalPolicy = null,
                    ILoyaltyCalculator? loyaltyCalculator = null)
    {
        // no-op: keep as data-only holder; adapters/services should be used for behaviour.
    }

    // Compatibility no-op for legacy callers that expect Customer to perform actions.
    public void MakePurchase()
    {
        // Intentionally no-op: actual purchase logic moved to services/adapters.
    }
}