Patch 19 — IPurchaseService adapter + SampleRunner migration

Summary
- Introduced `IPurchaseService` and `CustomerPurchaseService` adapter to allow callers to use a service instead of calling `Customer.MakePurchase()` directly.
- Updated `Samples/SampleRunner.cs` to use `CustomerPurchaseService` for the minor purchase demo, demonstrating a staged migration.
- Added unit test `CustomerPurchaseServiceTests`.

Why
- This reduces behavior tied to the `Customer` class and prepares for the planned conversion of `Customer` into a pure data holder. The adapter preserves existing behavior while enabling gradual migration.

Files added/changed
- Added: `Services/IPurchaseService.cs` (interface + `CustomerPurchaseService` adapter)
- Modified: `Samples/SampleRunner.cs` — uses adapter for minor purchase demo
- Added test: `CustomerPurchaseServiceTests.cs`

Verification
- Ran `dotnet test ApocalypticFastFood.sln`; build and tests succeeded.

Next steps
- Migrate other direct callers (tests or samples) as desired.
- Extract `ILoyaltyCalculator` next to decouple loyalty computation from `DiscountManager`.
