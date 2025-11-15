# Patch 8 — Customer behavior: policies and strategies (IAlcoholPolicy, IPurchaseApprovalPolicy, IDiscountStrategy, ILoyaltyCalculator)

Datum: 2025-11-15
Autor: (automatisch) Refactoring-Agent

## Kurzbeschreibung

Dieser Patch führt policy/strategy interfaces for customer behaviors and migrates `Customer`, `VipCustomer`, and `MinorCustomer` to delegate behavior to those policies. The change aims to eliminate exception-based control flow in `MinorCustomer`, prepare for flexible discount strategies, and make customer capabilities explicitly injectable for testing.

## Dateien geändert / hinzugefügt
- `Customer.cs` — added `CustomerContext`, interfaces, default implementations and refactored `Customer` to delegate behavior to injected policies while keeping legacy method signatures.
- `VIPCustomer.cs` — now delegates to base/policies instead of throwing exceptions.
- `MinorCustomer.cs` — delegates to base/policies; removed direct exceptions.
- `CustomerPolicies.cs` — added `MinorAlcoholPolicy`, `MinorPurchaseApprovalPolicy`, `VipDiscountStrategy`, and `VipLoyaltyCalculator` implementations.
- `ApocalypticFastFood.Tests/CustomerPolicyTests.cs` — tests verifying minor and VIP behaviors.
- `PATCH_8_DOCUMENTATION.md` — this document.

## Detaillierte Schritte
1. Add `CustomerContext` record to centralize customer state.
2. Add interfaces: `IDiscountStrategy`, `IAlcoholPolicy`, `IPurchaseApprovalPolicy`, `ILoyaltyCalculator`.
3. Provide default implementations that reproduce original `Customer` behavior.
4. Refactor `Customer` to accept policies via constructor and delegate behavior to them while exposing legacy methods.
5. Update `VipCustomer` and `MinorCustomer` to rely on the new policy system and stop throwing exceptions.
6. Add focused policy implementations for minors and VIP placeholders.
7. Add unit tests demonstrating expected behavior and parity where applicable.

## Tests & How to run
PowerShell (from workspace root):

```powershell
dotnet restore
dotnet test ApocalypticFastFood\ApocalypticFastFood.Tests\ApocalypticFastFood.Tests.csproj
```

Tests added: `CustomerPolicyTests`.

## Migration & Compatibility Notes
- Backwards-compatible: `Customer` methods keep the same signatures; existing code that constructs `new Customer()` or subclasses will continue to compile and run.
- Behavioral changes: `MinorCustomer` no longer throws exceptions inside `CanOrderAlcohol`/`MakePurchase`. Callers should check `CanOrderAlcohol()` or `MakePurchase` outcomes rather than rely on exceptions.
- Future improvement: inject specific policy instances via DI or constructors where domain-specific behavior is required.

## Rollback

```powershell
# if uncommitted
git checkout -- ApocalypticFastFood\ApocalypticFastFood\Customer.cs
git checkout -- ApocalypticFastFood\ApocalypticFastFood\VIPCustomer.cs
git checkout -- ApocalypticFastFood\ApocalypticFastFood\MinorCustomer.cs
rm ApocalypticFastFood\ApocalypticFastFood\CustomerPolicies.cs
rm ApocalypticFastFood\ApocalypticFastFood.Tests\CustomerPolicyTests.cs
```

## Commit-Message
`Refactor: add customer policy interfaces and migrate Customer/Vip/Minor to policy-based behavior; add tests`

## Next steps
- Replace direct subclass overrides with specific policy injections for `VipCustomer` and `MinorCustomer` where richer behavior is needed.
- Consider adding DI wiring for policies and moving policy implementations into own files/namespaces.

