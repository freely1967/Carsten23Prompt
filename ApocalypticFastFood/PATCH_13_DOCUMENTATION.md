# Patch 13 — Previous-Order Rule (Multiplier-aware)

Summary
- Extracted previous-order discount logic (`s19`) into `PreviousOrderRule` implementing `IDiscountRuleV2`.
- The rule applies when the current order (items sorted, joined by commas) equals `PreviousOrder` and follows the original nested conditions for days-since-last-visit, consecutive visits and membership levels.
- The rule can return both a discount amount and a multiplier (e.g., 35.0 discount with 1.25 multiplier for specific VIP cases), preserving original behavior.

Files changed/added
- `DiscountRules.cs` — added `PreviousOrderRule` (V2 rule).
- `DiscountManager.cs` — replaced the inlined `s19` block with a call to `DiscountEngineV2(new PreviousOrderRule()).Calculate(this)` and applied the returned discount + multiplier.
- `ApocalypticFastFood.Tests/PreviousOrderRuleTests.cs` — added unit tests for matching and non-matching cases.

How to run tests
```
cd c:\Users\elydu\Desktop\Carsten23Prompt\ApocalypticFastFood
dotnet test ApocalypticFastFood.sln
```

Rollback
- Revert the commit that adds `PreviousOrderRule` and the `DiscountEngineV2` call.
- Restore the original `s19` code block in `DiscountManager.cs`.

Next steps
- Continue migrating other multiplier-producing logic (referral-driven multipliers in `s5` or any remaining multiplier branches) into `IDiscountRuleV2` implementations.
- Run full test suite and resolve regressions.
