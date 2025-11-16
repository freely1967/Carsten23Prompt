Refactor progress for `refactor_plans_v1.json`

Summary: This document maps each entry from `refactor_plans_v1.json` to current status and actions taken.

1) `VIPCustomer.cs` — Strategy: Strategy Pattern (Medium)
- Status: Implemented.
- Actions: Added `IDiscountStrategy`/`VipDiscountStrategy` and injected it into `VipCustomer`. Added tests `VipCustomerStrategyTests.cs`.

2) `MinorCustomer.cs` — Strategy: policy objects (Low-Medium)
- Status: Implemented.
- Actions: Added `MinorAlcoholPolicy` and `MinorApprovalPolicy`; `MinorCustomer` now composes those policies. Added `MinorCustomerPolicyTests.cs`.

3) `Customer.cs` — Strategy: data-holder + services (High)
- Status: Planned / REVIEW patch created (PATCH_16_REVIEW_CUSTOMER_REFACTOR.md).
- Actions: Introduced `CustomerContext` and policy/service interfaces earlier; full conversion deferred as high-risk. Review patch outlines staged migration.

4) `Customer.cs` — Interface Segregation (Medium)
- Status: Implemented.
- Actions: Added `IOrderable` and `IAlcoholConsumer` and had `Customer` implement them (no breaking changes to callers).

5) `DiscountManager.cs` — Decompose into DiscountEngine & rules (Medium-High)
- Status: Partially implemented.
- Actions: Implemented `DiscountEngine` and `DiscountEngineV2`, many `IDiscountRule`/`IDiscountRuleV2` classes created (Promo, VisitCount, TimeOfDay, Family, Birthday, Streak, PreviousOrder, Referral). Zeroed legacy slots for migrated rules.
- Remaining: cleanup/refactor into smaller files is optional.

6) `DiscountManager.cs` — Pluggable IDiscountRule mapping (Low-Medium)
- Status: Partially implemented.
- Actions: Many condition blocks moved; engine iterates rules. Some legacy slots remain for non-migrated logic.

7) `PriceCatalog` (IPriceCatalog) (Low)
- Status: Implemented (`InMemoryPriceCatalog`).

8) `Dependency Inversion` for external integrations (IEmailSender/ISmsSender/IOrderRepository) (Low-Medium)
- Status: Implemented (Console adapters and DatabaseRepositoryAdapter present).

9) `IPaymentProcessor` segregation + facade (Low)
- Status: Implemented. `PaymentProcessorFacade` added; `CashPaymentProcessor` cleaned to implement only `ICashPaymentProcessor`.

10) `CashPaymentProcessor` Liskov fix (Low)
- Status: Implemented (cleanup completed).

11) `ReceiptFormatter` (SRP) (Low)
- Status: Implemented (`ReceiptFormatter` extracted and used in `DiscountManager`).

12) `Program.cs` demo extraction (Low)
- Status: Implemented (`Samples/SampleRunner.cs`).

Tests & Documentation
- Status: Many unit tests added for rules and policies; created `PATCH_1..PATCH_16` docs for applied/refactor steps.

Next recommended steps
- Approve the REVIEW plan in `PATCH_16_REVIEW_CUSTOMER_REFACTOR.md` to proceed with staged conversions for the high-risk `Customer` refactor.
- Add final integration tests for VIP+referral+birthday flows if behavioral parity needs to be certified.

If you'd like, I can now begin applying the staged adapters from the review plan (start with `IDiscountProvider` adapter and update one consumer). Reply with "apply staged customer refactor" to proceed.
