PR draft: refactor/solid-decomposition -> main

Summary:
This branch extracts and modularizes the monolithic discount logic, centralizes pricing, segregates payment interfaces, introduces a multiplier-capable discount engine (V2) and migrates multiple discount rules into testable classes. It also begins migrating customer policies and provides adapters to maintain backward compatibility. The goal is to improve SRP, Open/Closed, and Interface Segregation across the project while keeping behavior stable through unit tests.

Key changes (high level):
- Add `DiscountEngineV2`, `DiscountResult`, `IDiscountRuleV2` to support rules that return both discounts and multipliers.
- Migrate many discount rules to rule classes: `PromoCodeRule`, `VisitCountRule`/`VisitCountRuleV2`, `TimeOfDayRule`, `FamilyRule`, `BirthdayRule` (V2), `StreakRule` (V2), `PreviousOrderRule` (V2), `ReferralRuleV2`.
- Zero legacy `sX` slots upon migration to avoid double-counting.
- Centralize item pricing in `IPriceCatalog` and implement `InMemoryPriceCatalog`.
- Segregate payment interfaces (small interfaces + `PaymentProcessorFacade`) and clean `CashPaymentProcessor` to implement `ICashPaymentProcessor` only.
- Extract `ReceiptFormatter` and `SampleRunner` for demo code.
- Introduce `CustomerContext` and policy/strategy interfaces; add `VipDiscountStrategy` and compose it into `VipCustomer`.

Files changed (representative):
- ApocalypticFastFood/ApocalypticFastFood/DiscountEngine.cs
- ApocalypticFastFood/ApocalypticFastFood/DiscountManager.cs
- ApocalypticFastFood/ApocalypticFastFood/DiscountRules.cs
- ApocalypticFastFood/ApocalypticFastFood/Customer.cs
- ApocalypticFastFood/ApocalypticFastFood/VIPCustomer.cs
- ApocalypticFastFood/ApocalypticFastFood/MinorCustomer.cs
- ApocalypticFastFood/ApocalypticFastFood/Program.cs
- ApocalypticFastFood/ApocalypticFastFood/Samples/SampleRunner.cs
- ApocalypticFastFood/ApocalypticFastFood.Tests/* (many rule/unit/integration tests)
- Documentation: `PATCH_1..PATCH_15_DOCUMENTATION.md`, `PR_DESCRIPTION.md`

Testing:
- Unit tests were added for migrated rules; I ran `dotnet test` during the refactor batches and builds/tests compiled successfully.

Migration notes & risk:
- Customer-model changes are higher risk (API shape changes). The branch keeps backward-compatible adapters (default strategies/policies) so existing constructions keep working.
- Payment interface segregation preserves a `PaymentProcessorFacade` so callers using the legacy `IPaymentProcessor` remain supported.

Open items remaining (recommended follow-ups):
- Implement minor-customer policies (approval/alcohol) and remove exception-throwing behavior.
- Finalize VIP discount logic based on precise business rules (currently conservative stub returns 0.0).
- Add integration tests covering end-to-end scenarios for VIP + referral + birthday multipliers to ensure behavioral parity.

How to test locally:

```powershell
cd c:\Users\elydu\Desktop\Carsten23Prompt\ApocalypticFastFood
dotnet test ApocalypticFastFood.sln
```

Suggested reviewers:
- Backend / C# architecture lead
- QA engineer for discount calculations
- DevOps for CI verification

---

(If you want, I can open the PR on GitHub for you and attach the PATCH_*.md documents as references.)
