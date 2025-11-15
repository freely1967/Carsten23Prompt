PR: Refactor - SOLID decomposition and rule engine migration

Summary

This PR contains an incremental, test-covered refactor of the `ApocalypticFastFood` project to address SOLID issues and make the codebase more maintainable. Changes are grouped into small, reviewable patches (Patch 1..Patch 10). All changes are additive or guarded to preserve existing behavior while enabling gradual migration.

Branch: `refactor/solid-decomposition`

High-level changes (by patch)

- Patch 1: Centralize pricing with `IPriceCatalog` and `InMemoryPriceCatalog`; refactor `OrderProcessor` to use the price catalog; add `PriceCatalogTests`.
- Patch 2: Segregate payment interfaces, add `PaymentProcessorFacade`, refactor `OrderProcessor` to accept `IPaymentProcessor`, add tests and docs.
- Patch 3: Add test for `CardPaymentProcessor` delegation and documentation.
- Patch 4: Introduce `DiscountEngine` + `IDiscountRule` and two initial rules `PromoCodeRule` and `VisitCountRule` with tests.
- Patch 5: Wire `PromoCodeRule` into `DiscountManager.CalculateDiscount` (guarded) and add integration test.
- Patch 6: Wire `VisitCountRule` into `CalculateDiscount` (guarded) and add integration test.
- Patch 7: Add `TimeOfDayRule` and wire into `CalculateDiscount` (suppresses legacy s2/s8); add integration test.
- Patch 8: Extract customer behavior into policies/strategies (`CustomerContext`, `IAlcoholPolicy`, `IPurchaseApprovalPolicy`, `IDiscountStrategy`, `ILoyaltyCalculator`), refactor `Customer` and subclasses, add tests.
- Patch 9: Move demo scenarios from `Program.cs` into `Samples/SampleRunner.cs` and add smoke test.
- Patch 10: Add `FamilyRule` for family/kids discounts and wire into `CalculateDiscount` (guarded); add integration test.

Files added/modified (not exhaustive)
- `DiscountManager.cs` (many guarded changes, engine wiring)
- `DiscountEngine.cs`, `DiscountRules.cs` (new engine & rules)
- `Customer.cs`, `CustomerPolicies.cs`, `VIPCustomer.cs`, `MinorCustomer.cs` (policy refactor)
- `ApocalypticFastFood.Tests/*` (many new tests: PriceCatalogTests, PaymentProcessorTests, OrderProcessorPaymentTests, DiscountEngineTests, DiscountManager*IntegrationTests, ReceiptFormatterTests, CustomerPolicyTests, SampleRunnerTests)
- `Samples/SampleRunner.cs` (moved demos)
- `PATCH_*.md` documents for each patch

Tests

Run locally (PowerShell):

```powershell
cd c:\Users\elydu\Desktop\Carsten23Prompt
dotnet restore
dotnet test ApocalypticFastFood\ApocalypticFastFood.Tests\ApocalypticFastFood.Tests.csproj
```

PR Checklist (recommended)
- [ ] Run all unit tests locally and ensure green in CI
- [ ] Review each `PATCH_*.md` to understand scope and migration notes
- [ ] Review `DiscountManager.CalculateDiscount()` changes carefully for parity
- [ ] If acceptable, push branch and create PR with `gh pr create --fill` or open PR in GitHub web

How to push & create PR (PowerShell)

```powershell
cd c:\Users\elydu\Desktop\Carsten23Prompt
# push branch to origin
git push -u origin refactor/solid-decomposition
# create PR with GitHub CLI if available
gh pr create --fill
```

Notes

- This PR intentionally used additive, guarded changes (the engine is introduced and used rule-by-rule) to make verification and rollback straightforward.
- For rules that affect multiplier behavior (e.g., birthday/streaks), I recommend a small design discussion to decide whether `IDiscountRule` should be extended to return both discount and multiplier adjustments.

If you'd like, I can push the branch for you (requires configured remote & credentials), or I can open a PR draft using `gh` if it's installed and authenticated on your machine. After the PR is created, I'll continue migrating the next rule group (birthday/multiplier) as planned.
