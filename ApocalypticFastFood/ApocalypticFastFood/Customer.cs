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

    // Parameterless data-only constructor (preferred for new code)
    public Customer()
    {
    }

    /// <summary>
    /// Compatibility constructor: accepts legacy policy/strategy params but keeps Customer as a data holder.
    /// These parameters are ignored and kept for binary/constructor compatibility with older callers.
    /// </summary>
    [System.Obsolete("Use parameterless constructor. Behavioral dependencies must be injected in services/adapters.")]
    public Customer(IDiscountStrategy? discountStrategy = null,
                    IAlcoholPolicy? alcoholPolicy = null,
                    IPurchaseApprovalPolicy? purchaseApprovalPolicy = null,
                    ILoyaltyCalculator? loyaltyCalculator = null)
    {
        // no-op: keep as data-only holder; adapters/services should be used for behaviour.
    }

    /// <summary>
    /// Compatibility no-op for legacy callers that expect Customer to perform actions.
    /// Use an IPurchaseService implementation for actual purchase behavior.
    /// </summary>
    [System.Obsolete("Use IPurchaseService.MakePurchase(CustomerContext) instead of calling Customer.MakePurchase().")]
    public void MakePurchase()
    {
        // Intentionally left as no-op for binary compatibility. Consider removing in a later major version.
    }
}