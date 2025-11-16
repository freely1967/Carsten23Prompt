# Patch 12 — Streak Rule (Multiplier-aware)

Summary
- Introduced `StreakRule` as an `IDiscountRuleV2` implementation that encapsulates the legacy `s12` streak-based discount and multiplier logic.
- Replaced the inlined `s12` block in `DiscountManager.CalculateDiscount()` with a call to `DiscountEngineV2` that applies `StreakRule`.
- Added unit tests `StreakRuleTests` validating both multiplier-producing and standard cases.

Files changed/added
- `DiscountRules.cs` — added `StreakRule` (V2 rule).
- `DiscountEngine.cs` — replaced legacy `s12` logic with `DiscountEngineV2(new StreakRule())` invocation.
- `ApocalypticFastFood.Tests/StreakRuleTests.cs` — unit tests for `StreakRule`.
- `PATCH_12_DOCUMENTATION.md` — this file.

How to run tests
```
cd c:\Users\elydu\Desktop\Carsten23Prompt\ApocalypticFastFood
dotnet test ApocalypticFastFood.sln
```

Rollback
- Revert the commit that added `StreakRule` and the `DiscountEngineV2` invocation.
- Restore the original `s12` code block in `DiscountManager.CalculateDiscount()`.

Next steps
- Migrate other multiplier-producing segments (previous-order s19, referral s5 if applicable) into `IDiscountRuleV2` implementations.
- Run full test suite and fix any regressions.
