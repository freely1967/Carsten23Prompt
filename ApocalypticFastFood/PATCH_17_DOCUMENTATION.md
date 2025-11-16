Patch 17 — CashPaymentProcessor test + verification

What I inspected
- `DiscountManager.cs` — located payment processor interfaces and `CashPaymentProcessor` implementation.
- `Customer.cs`, `CustomerPolicies.cs`, `VIPCustomer.cs` — inspected discount strategy wiring.
- `refactor_plans_v1.json` — reviewed remaining planned refactors.

What I changed
- Added unit test: `ApocalypticFastFood.Tests/CashPaymentProcessorTests.cs` which calls `CashPaymentProcessor.ProcessCash(10.0)` to ensure no exception.

Why
- The refactor plan requested cleaning `CashPaymentProcessor` first. The codebase already contained a segregated `ICashPaymentProcessor` implementation without thrown `NotImplementedException`s, so no behavioral cleanup was required. To be thorough I added an automated test to prevent regressions.

Verification
- Ran `dotnet test ApocalypticFastFood.sln` — build completed successfully and tests ran (no build errors reported).

Notes on VIPCustomer → IDiscountStrategy
- The repository already contains `IDiscountStrategy`, `VipDiscountStrategy`, and `VipCustomer` composes the `VipDiscountStrategy` via constructor injection. Unit tests for the VIP path are present (`VipCustomerStrategyTests.cs`). No further code changes were necessary for that refactor.

Remaining higher-risk work (from `refactor_plans_v1.json`)
- Stage-2: Migrate `OrderProcessor` to accept `IDiscountProvider` (adapter-based transition).
- Add `DiscountManagerDiscountProvider` adapter to expose `DiscountManager` as `IDiscountProvider`.
- Introduce `IPurchaseService` and migrate callers to it.
- Extract `ILoyaltyCalculator` and migrate loyalty logic.
- Final conversion: turn `Customer` into pure data holder after all consumers use the new services (high risk — review patch required).
- Prepare PR checklist and run smoke integration tests covering VIP+referral+birthday cases.

Next recommended action
- If you want me to continue: I can implement Stage-2 by adding `DiscountManagerDiscountProvider`, updating `OrderProcessor` to accept `IDiscountProvider` (with adapter fallback), add tests, and run the test suite. This is the next lowest-risk refactor and will enable the final Customer conversion.
