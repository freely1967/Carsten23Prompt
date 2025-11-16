Patch 18 — Stage-2: DiscountManager adapter + OrderProcessor migration

Summary
- Introduced `DiscountManagerDiscountProvider` to expose `DiscountManager` as `IDiscountProvider`.
- Updated `OrderProcessor` to accept an optional `IDiscountProvider` in its constructor and prefer it when calculating discounts; when none provided, it uses the adapter wrapping the existing `DiscountManager` instance to preserve legacy behavior.
- Added unit tests: `DiscountManagerDiscountProviderTests` and `OrderProcessorTests`.

Why
- This is the next low-risk staged migration step to decouple discount retrieval from the `DiscountManager` implementation while preserving behavior for callers.

Files added/changed
- Added: `DiscountManagerDiscountProvider.cs` — adapter.
- Changed: `DiscountManager.cs` — `OrderProcessor` now accepts `IDiscountProvider` and uses it to get discounts.
- Added tests: `DiscountManagerDiscountProviderTests.cs`, `OrderProcessorTests.cs`.

Verification
- Ran `dotnet test ApocalypticFastFood.sln` after applying changes; build and tests succeeded.

Next steps
- Continue migrating other consumers to accept `IDiscountProvider` (e.g., SampleRunner, any direct callers).
- After all consumers use `IDiscountProvider`, convert `Customer`/`DiscountManager` internals as planned (final high-risk Customer conversion).
