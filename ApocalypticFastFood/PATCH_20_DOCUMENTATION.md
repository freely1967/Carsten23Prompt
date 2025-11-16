Patch 20 — Extract loyalty points calculation to `ILoyaltyPointsCalculator`

Summary
- Extracted loyalty points computation from `DiscountManager` into `ILoyaltyPointsCalculator` and `DefaultLoyaltyPointsCalculator`.
- `DiscountManager` now accepts an optional `ILoyaltyPointsCalculator` via constructor and delegates `CalculateLoyaltyPoints()` to it. The default behaviour is preserved by `DefaultLoyaltyPointsCalculator`.

Why
- This decouples a significant business rule (loyalty points) from the large `DiscountManager` class, improving testability and making the codebase ready for the final `Customer` migration.

Files added/changed
- Added: `Services/ILoyaltyPointsCalculator.cs` (interface + `DefaultLoyaltyPointsCalculator`).
- Modified: `DiscountManager.cs` — added constructor injection and delegated loyalty calculation.
- Added test: `DefaultLoyaltyPointsCalculatorTests.cs`.

Verification
- Ran `dotnet test ApocalypticFastFood.sln` — build and tests succeeded.

Next steps
- Migrate remaining consumers to adapters/services if any remain.
- Prepare review patch for final `Customer` conversion once all consumers use service adapters.
