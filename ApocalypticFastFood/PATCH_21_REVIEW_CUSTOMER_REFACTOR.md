Patch 21 — REVIEW: Final `Customer` → pure data-holder conversion (High-risk)

Purpose
- This is a review-only patch describing the final, high-risk migration that converts `Customer` from a behaviour-rich class into a pure data holder. Do NOT apply this patch automatically — it must be reviewed by domain experts before making code changes.

Summary
- Goal: Move all behavioral responsibilities (discount calculation, purchase side-effects, alcohol checks, loyalty multipliers) out of `Customer` and into narrow, well-tested services (`IDiscountProvider`, `IPurchaseService`, `IAlcoholPolicy`, `ILoyaltyPointsCalculator`) and then change `Customer` to a simple POCO with properties only.
- Reason: current `Customer` mixes state and behavior and blocks SRP / testability. We already created adapters and services to preserve behavior during staged migration; this final conversion eliminates the remaining behavioural members and simplifies the model.

High-level migration steps
1) Verify prerequisites
   - All production code that previously relied on `Customer` behaviour must instead consume the service adapters introduced earlier: `IDiscountProvider` (we added `DiscountManagerDiscountProvider`), `IPurchaseService` (`CustomerPurchaseService`), and `ILoyaltyPointsCalculator` (`DefaultLoyaltyPointsCalculator`).
   - Ensure `Samples/*` and `OrderProcessor` are migrated (done).
   - Ensure code that intentionally tests `Customer` behaviour remains in unit tests for the services rather than on `Customer` itself.

2) Create review patch (this document)
   - Provide unified-diff illustrating the intended transformation in `Customer.cs` and example adapter wiring changes for `VipCustomer` and `MinorCustomer`.

3) Run integration & smoke tests
   - Run all unit and integration tests locally and in CI.
   - Run `Samples.SampleRunner.RunDemos()` to manually validate sample outputs.

4) Apply conversion (only after approvals)
   - Replace `Customer` implementation with auto-properties (data-only). Remove or obsolete behavioural methods: `GetDiscount()`, `MakePurchase()`, `CanOrderAlcohol()`, `GetLoyaltyMultiplier()`.
   - Where necessary, keep compatibility shims (small adapters) for external packages; prefer deprecation with warnings.

5) Post-change verification
   - Run full test-suite, run smoke integration tests and sample runner.
   - Request domain review on VIP/loyalty business rules.

Unified diff (review-only)
--- a/ApocalypticFastFood/Customer.cs
+++ b/ApocalypticFastFood/Customer.cs
@@
 -public class Customer : IOrderable, IAlcoholConsumer
 -{
 -    // lightweight state kept for backwards compatibility
 -    public int Id { get; set; }
 -    public int Age { get; set; }
 -    public int VisitCount { get; set; }
 -    public string MembershipLevel { get; set; } = string.Empty;
 -    public bool HasParentApproval { get; set; }
 -
 -    private readonly IDiscountStrategy _discountStrategy;
 -    private readonly IAlcoholPolicy _alcoholPolicy;
 -    private readonly IPurchaseApprovalPolicy _purchaseApprovalPolicy;
 -    private readonly ILoyaltyCalculator _loyaltyCalculator;
 -
 -    public Customer(IDiscountStrategy? discountStrategy = null,
 -                    IAlcoholPolicy? alcoholPolicy = null,
 -                    IPurchaseApprovalPolicy? purchaseApprovalPolicy = null,
 -                    ILoyaltyCalculator? loyaltyCalculator = null)
 -    {
 -        _discountStrategy = discountStrategy ?? new DefaultDiscountStrategy();
 -        _alcoholPolicy = alcoholPolicy ?? new DefaultAlcoholPolicy();
 -        _purchaseApprovalPolicy = purchaseApprovalPolicy ?? new DefaultPurchaseApprovalPolicy();
 -        _loyaltyCalculator = loyaltyCalculator ?? new DefaultLoyaltyCalculator();
 -    }
 -
 -    // Backwards-compatible method signatures: delegate to injected policies using current state
 -    public virtual double GetDiscount()
 -    {
 -        var ctx = new CustomerContext(Id, Age, VisitCount, MembershipLevel, HasParentApproval);
 -        return _discountStrategy.GetDiscount(ctx);
 -    }
 -
 -    public virtual void MakePurchase()
 -    {
 -        var ctx = new CustomerContext(Id, Age, VisitCount, MembershipLevel, HasParentApproval);
 -        if (!_purchaseApprovalPolicy.CanMakePurchase(ctx))
 -        {
 -            Console.WriteLine("Purchase not approved for customer " + Id);
 -            return;
 -        }
 -
 -        // purchase side-effects would happen here in real implementation
 -    }
 -
 -    public virtual bool CanOrderAlcohol()
 -    {
 -        var ctx = new CustomerContext(Id, Age, VisitCount, MembershipLevel, HasParentApproval);
 -        return _alcoholPolicy.CanOrderAlcohol(ctx);
 -    }
 -
 -    public virtual int GetLoyaltyMultiplier()
 -    {
 -        var ctx = new CustomerContext(Id, Age, VisitCount, MembershipLevel, HasParentApproval);
 -        return _loyaltyCalculator.GetMultiplier(ctx);
 -    }
 -}
 +public class Customer
 +{
 +    // Data-only representation used across the codebase
 +    public int Id { get; set; }
 +    public int Age { get; set; }
 +    public int VisitCount { get; set; }
 +    public string MembershipLevel { get; set; } = string.Empty;
 +    public bool HasParentApproval { get; set; }
 +}

Migration wiring examples (callers)
- `OrderProcessor` already accepts `IDiscountProvider` and uses `DiscountManagerDiscountProvider` when no provider is injected.
- Any previous call `customer.GetDiscount()` should be replaced with:
  - Construct `CustomerContext ctx` from `customer` and call the available `IDiscountProvider.GetDiscount(ctx)`.

Adapter example (already present)
- `CustomerDiscountProvider` preserves legacy `Customer` behaviour by mapping `CustomerContext` into the wrapped `Customer` instance and calling `GetDiscount()`; keep this adapter during migration but prefer service usage in new code.

Open questions / Domain review required
- VIP business rules: `VipDiscountStrategy` currently returns 0 to avoid invented logic. Domain team must confirm intended VIP discounts.
- Loyalty multipliers: `VipLoyaltyCalculator` in `CustomerPolicies.cs` returns `-1` (legacy odd behavior). Confirm whether this is intentional and what the correct multiplier is.
- Exceptional flows: `Customer.MakePurchase()` used to throw for minors — now replaced by policies and adapters. Confirm desired behavior on blocked purchases (throw vs logging).

Risk & mitigation
- Risk: High — API shape change will break any external consumers relying on `Customer` methods. Mitigation: keep adapters (`CustomerDiscountProvider`, `CustomerPurchaseService`) and perform staged migration of callers; produce deprecation warnings and doc.

Rollback plan
- Revert commit(s) (single PR) to previous `Customer` implementation. Because adapters and service interfaces are additive and backwards-compatible, revert should be straightforward.

Required test matrix
- Unit tests: ensure service implementations (`DiscountEngineV2`, `DefaultLoyaltyPointsCalculator`, `CustomerPurchaseService`, `CustomerDiscountProvider`) have full coverage.
- Integration tests: run existing `DiscountManager*IntegrationTests` and `OrderProcessor` tests.
- Smoke tests: run `Samples.SampleRunner.RunDemos()` and manual spot-checks for VIP/Referral/Birthday scenarios.

Commit message suggestion
- "Finalize Customer conversion planning: review patch and migration plan for converting Customer into a pure data-holder (HIGH RISK)"

Notes
- Do NOT apply code transforms automatically without domain approval. Create a PR with this review patch and request reviewers from product/domain owners.
