PATCH 16 — REVIEW: Major Customer model refactor (High risk)

Goal
- Turn `Customer` into a data-only holder (`CustomerContext`) and move behaviors into focused service interfaces (e.g. `IDiscountProvider`, `IPurchaseService`, `ILoyaltyCalculator`).

Why a review patch
- This is a large, API-shaping change (High risk). To avoid breaking existing call sites and consumers, we provide a review patch + migration plan rather than applying the change directly.

Proposed strategy (summary)
- Introduce `CustomerContext` record (already present).
- Add service interfaces:
  - `IDiscountProvider { double GetDiscount(CustomerContext ctx); }`
  - `IPurchaseService { void MakePurchase(CustomerContext ctx); }`
  - `ILoyaltyCalculator { int GetMultiplier(CustomerContext ctx); }`
- Provide default adapters that wrap current `Customer` behavior so callers can be migrated incrementally.
- Replace internal `Customer` method logic by delegating to the services via DI, one service at a time.

Stepwise migration plan
1) Add `IDiscountProvider`, `IPurchaseService`, and `ILoyaltyCalculator` interfaces and default implementations that delegate to existing `Customer` methods.
2) Add constructor overloads to `Customer` to accept these services (with defaults preserving current behavior).
3) Update one consumer (e.g., `OrderProcessor`) to use `IDiscountProvider` instead of calling `Customer.GetDiscount()` directly and add tests.
4) Repeat migrating other consumers until `Customer` methods are not used directly; once stable, replace `Customer` methods with a thin shim or deprecate them.
5) Final step: if desired, convert `Customer` class to a pure data holder and rename service implementations appropriately.

Proposed unified-diff example (REVIEW only)
--- a/ApocalypticFastFood/Customer.cs
+++ b/ApocalypticFastFood/Customer.cs
@@
-public class Customer : IOrderable, IAlcoholConsumer
+public class Customer : IOrderable, IAlcoholConsumer
 {
-    // current fields and constructors
+    // fields unchanged
+    private readonly IDiscountProvider _discountProvider;
+    private readonly IPurchaseService _purchaseService;
+    private readonly ILoyaltyCalculator _loyaltyCalculator;
@@
-    public virtual double GetDiscount()
-    {
-        var ctx = new CustomerContext(Id, Age, VisitCount, MembershipLevel, HasParentApproval);
-        return _discountStrategy.GetDiscount(ctx);
-    }
+    public virtual double GetDiscount()
+    {
+        var ctx = new CustomerContext(Id, Age, VisitCount, MembershipLevel, HasParentApproval);
+        return _discountProvider.GetDiscount(ctx);
+    }
@@
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
+    public virtual void MakePurchase()
+    {
+        var ctx = new CustomerContext(Id, Age, VisitCount, MembershipLevel, HasParentApproval);
+        _purchaseService.MakePurchase(ctx);
+    }

[This is an illustrative snippet. The full application should be staged and tested as described in the migration plan.]

Tests
- Create adapter tests that verify the default `IDiscountProvider` delegates to the previous `Customer.GetDiscount()` implementation. Do not remove original methods until all consumers are migrated and tests pass.

Risk & Migration notes
- Risk: High — breaks public API and constructors. Mitigation: provide constructor overloads/default adapters and migrate callers incrementally.
- Ensure a comprehensive test-suite (unit + integration) is present before finishing.

Rollback
- Revert the commit(s) that apply the final data-holder conversion; the staged approach keeps changes small and revertable at each step.

Commit message suggestion
- "REVIEW: plan for Customer → data-holder migration; add adapters and migration steps (no behavior change)"

Open questions (need your input)
- Do you want the final `Customer` conversion applied automatically once all consumers are migrated, or do you prefer keeping the compatibility shim longer for a phased rollout?
- Are there external consumers (outside this repo) that rely on the shape of `Customer` we should keep in mind?

If you approve this review plan I will (on your instruction) apply the staged adapter patches one-by-one, starting with `IDiscountProvider` adapter and updating `OrderProcessor` to use it.
