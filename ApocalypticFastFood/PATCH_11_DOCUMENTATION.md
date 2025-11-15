# Patch 11 — Birthday Rule (Multiplier-aware)

Summary
- Added `DiscountResult` and `DiscountEngineV2` to support rules that return both a discount amount and a multiplier.
- Implemented `BirthdayRule` as an `IDiscountRuleV2` providing both a discount and a multiplier (extracted from legacy `s7` logic).
- Added unit tests `BirthdayRuleTests` to validate the `BirthdayRule` behavior.

Why
- Some legacy discount logic both adds a flat discount and multiplies the total discount (e.g., birthday bonuses). To migrate safely, we introduced a non-breaking V2 engine that can return both values.

Files changed/added
- `DiscountEngine.cs` — added `DiscountResult`, `IDiscountRuleV2`, and `DiscountEngineV2`.
- `DiscountRules.cs` — added `BirthdayRule` (V2 rule).
- `ApocalypticFastFood.Tests/BirthdayRuleTests.cs` — new unit tests for the birthday rule.

How to run tests
From repository root (PowerShell):
```
cd c:\Users\elydu\Desktop\Carsten23Prompt\ApocalypticFastFood
dotnet test ApocalypticFastFood.sln
```

Rollback
- To rollback this patch:
  - Revert the commit that adds `DiscountEngineV2`, `DiscountResult`, and `BirthdayRule`.
  - Restore the original inlined `s7` logic in `DiscountEngine.cs` if necessary.

Next steps
- Add unit and integration tests that assert end-to-end effects of multipliers when applied via `DiscountManager.CalculateDiscount()`.
- Migrate other multiplier-producing sections (streaks, previous-order, referral) into `IDiscountRuleV2` implementations and tests.
