# PATCH 15 — ReferralRuleV2 Migration

Summary
- Added `ReferralRuleV2` to migrate legacy referral logic (s10) into the `DiscountEngineV2` rule engine.
- Wired `DiscountManager.CalculateDiscount()` to call `DiscountEngineV2` for referrals, zeroed legacy `s10` slot, and applied returned `DiscountResult` (discount and multiplier).
- Added `ReferralRuleV2Tests.cs` with three xUnit tests covering primary referral scenarios.

Files changed
- `ApocalypticFastFood/DiscountRules.cs` — added `ReferralRuleV2` (IDiscountRuleV2 implementation).
- `ApocalypticFastFood/DiscountManager.cs` — replaced legacy referral switch with `DiscountEngineV2` invocation, zeroed `s10`, applied returned discount and multiplier.
- `ApocalypticFastFood.Tests/ReferralRuleV2Tests.cs` — new unit tests.

Rationale
- This removes logic duplication and moves referral rules to the rule engine alongside other discount rules. It prevents double-counting by zeroing the legacy slot and centralizes multiplier logic through `DiscountEngineV2`.

Rollback
- Revert commit that contains these changes. The legacy referral switch was removed, so reverting will restore previous behavior.

Notes
- Tests were run locally (solution builds successfully). Next: push changes and migrate remaining multiplier occurrences (birthday and streak blocks) to use `DiscountEngineV2` and zero legacy slots.
