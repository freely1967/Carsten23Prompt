Patch 21 — FINALIZED: `Customer` converted to a pure data-holder; compatibility adapters added

Summary of changes (applied)
- The final high-risk conversion of `Customer` from a behaviour-rich class into a simple data holder has been applied in this branch. `Customer` is now a POCO with auto-properties and an additive compatibility constructor to preserve existing constructor call sites.
- To preserve runtime behaviour and enable a staged migration, the following compatibility artifacts were added:
  - `CustomerContracts.cs` (new): extracted interfaces and contracts previously embedded in `Customer` (e.g. `IDiscountStrategy`, `IAlcoholPolicy`, `IPurchaseApprovalPolicy`, `ILoyaltyCalculator`, `IDiscountProvider`).
  - `Adapters/CustomerDiscountProvider.cs` (new): maps a data-only `Customer` into a `DiscountManager` and delegates discount calculation to preserve legacy semantics used by samples/tests.
  - `Adapters/DiscountManagerDiscountProvider.cs` (moved): the adapter that exposes the existing `DiscountManager` as an `IDiscountProvider` has been centralized under `Adapters/` (previously at repository root).

Validation performed
- Ran `dotnet test ApocalypticFastFood.sln` after applying the changes. The solution restores and builds successfully.
- Compatibility constructor on `Customer` preserves existing subclass constructors such as `VipCustomer()` which call `base(...)`.

Migration notes and remaining tasks
1) Contract centralization
   - `CustomerContracts.cs` contains the interface definitions; implementations remain in `CustomerPolicies.cs` and other files. Review these implementations to ensure they target the extracted interfaces.

2) Adapter centralization
   - `Adapters/CustomerDiscountProvider.cs` and `Adapters/DiscountManagerDiscountProvider.cs` provide two migration paths: (a) wrap an existing `Customer` and route to `DiscountManager`, and (b) adapt `DiscountManager` directly to `IDiscountProvider` for new callers. Keep both during migration and prefer consumers to depend on `IDiscountProvider`.

3) VIP / loyalty domain review
   - `VipDiscountStrategy` (in `CustomerPolicies.cs`) is conservative (returns `0.0`) to avoid introducing new business behavior. Domain owners must specify intended VIP discounts.
   - `VipLoyaltyCalculator` currently returns `-1` (legacy oddity). Confirm intended multiplier semantics; update `DefaultLoyaltyPointsCalculator` or `VipLoyaltyCalculator` accordingly.

4) Tests and callers
   - Existing unit tests compiled. Some tests and samples still use `CustomerDiscountProvider` or `DiscountManagerDiscountProvider` — these have been moved/added under `Adapters/`. Update any import/usings if an IDE flags them.

5) Cleanup and follow-ups
   - Move/centralize any remaining small adapters into the `Adapters/` folder for discoverability.
   - Replace direct `Customer` behavior usage in code and tests with service interfaces (`IDiscountProvider`, `IPurchaseService`, `ILoyaltyPointsCalculator`) incrementally.

Open questions (for domain team)
- Confirm VIP discount rules and multipliers.
- Confirm desired behavior for blocked purchases (throw vs. non-throw + logging) for minors and other exceptional cases.

Risk & rollback
- Risk: High — this was an API-shaping change. Mitigations applied: adapters added, a compatibility constructor retained, and contract interfaces extracted.
- Rollback: revert the PR/commits containing these changes. Because adapters and interfaces are additive, rolling back the `Customer` replacement will restore previous behaviour.

Suggested next steps
- Run the full test suite (including integration tests) in CI and request domain review on VIP/loyalty behavior.
- After domain sign-off, remove compatibility shims gradually and update call sites to depend directly on service interfaces.

Commit message (example)
- "Apply final Customer conversion: replace behaviour with data-only POCO and add compatibility contracts/adapters (Adapters/, CustomerContracts.cs)"

Notes
- This file documents the review and the actions already applied in this branch. If you want me to proceed with additional cleanup (centralize remaining adapters, update docs, or open the PR), tell me which item to do next.
