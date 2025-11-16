# Patch 14 — VisitCount Multiplier Migration (s5)

Summary
- Extracted visit-count multiplier logic from the legacy `s5` block into `VisitCountRuleV2` (implements `IDiscountRuleV2`).
- Kept the flat visit discounts in the existing `VisitCountRule` (IDiscountRule) to avoid double-counting; the new V2 rule only produces multiplier adjustments.
- Wired `VisitCountRuleV2` into `DiscountManager.CalculateDiscount()` using `DiscountEngineV2` and applied the returned multiplier.
- Added unit tests `VisitCountRuleV2Tests` covering major multiplier branches.

Files changed/added
- `DiscountRules.cs` — added `VisitCountRuleV2`.
- `DiscountManager.cs` — after computing `visitDiscount`, invoked `DiscountEngineV2(new VisitCountRuleV2()).Calculate(this)` and applied its multiplier.
- `ApocalypticFastFood.Tests/VisitCountRuleV2Tests.cs` — tests for high-visit multiplier cases.
- `PATCH_14_DOCUMENTATION.md` — this file.

How to run tests
```
cd c:\Users\elydu\Desktop\Carsten23Prompt\ApocalypticFastFood
dotnet test ApocalypticFastFood.sln
```

Rollback
- Revert the commit that adds `VisitCountRuleV2` and the `DiscountEngineV2` invocation, and restore the original multiplier lines in `DiscountManager.CalculateDiscount()`.

Notes & Rationale
- This approach keeps flat discounts and multipliers separate: `VisitCountRule` supplies the numeric discount amounts, `VisitCountRuleV2` supplies multiplier adjustments. This avoids altering the already-migrated visit discount behavior while restoring multiplier semantics.

Next steps
- Migrate any remaining multiplier-producing blocks (e.g., any referral-related multiplier logic elsewhere) and run the full test suite.
