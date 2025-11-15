# Patch 10 — Family & Kids discounts: FamilyRule and migration

Datum: 2025-11-15
Autor: (automatisch) Refactoring-Agent

## Kurzbeschreibung

Dieser Patch extrahiert die Familien-/Kinder-Rabattlogik aus dem monolithischen `CalculateDiscount()` in eine neue `FamilyRule : IDiscountRule` und integriert diese Regel in die `DiscountEngine`. Die legacy-Beitragsvariable `s4` wird bei migrierten Fällen auf `0` gesetzt, und `familyDiscount` wird der endgültigen Rabatt-Summe hinzugefügt.

## Dateien geändert / hinzugefügt
- `ApocalypticFastFood/ApocalypticFastFood/DiscountRules.cs` — neue `FamilyRule` implementiert.
- `ApocalypticFastFood/ApocalypticFastFood/DiscountManager.cs` — berechnet `familyDiscount` via `DiscountEngine` und setzt `s4 = 0` für migrierte Fälle; `familyDiscount` wird zur Endsumme addiert.
- `ApocalypticFastFood/ApocalypticFastFood.Tests/DiscountManagerFamilyIntegrationTests.cs` — Integrationstest zur Paritätsprüfung.
- `PATCH_10_DOCUMENTATION.md` — dieses Dokument.

## Schritte (ausführlich)
1. Add `FamilyRule` that mirrors the existing s4 logic (FamilyMembers >= 4, HasKids, ItemCount >= 8, weekend/holiday/time checks).
2. In `CalculateDiscount()`, compute `familyDiscount` with `new DiscountEngine(new[] { new FamilyRule() }).Calculate(this)` when `FamilyMembers >= 4`.
3. Set `s4 = 0.0` when `FamilyMembers >= 4` to prevent double-counting.
4. Add `familyDiscount` to the aggregated `discount` (the spot where promo/visit/time discounts are already added).
5. Add an integration test verifying `CalculateDiscount()` parity for a representative family scenario.

## Tests
- `DiscountManagerFamilyIntegrationTests.CalculateDiscount_Uses_FamilyEngine_WhenFamilyPresent` verifies that for a representative family/holiday case `dm.CalculateDiscount()` matches `DiscountEngine.Calculate()`.

## Migration & Risk
- Risk: Low-Medium. Additive and guarded; existing behavior when `FamilyMembers < 4` remains unchanged. Edge cases where multiple rules interact should be validated in later patches.
- Migration plan: Continue migrating related rules (e.g., birthday multipliers, referral modifiers) and then consider consolidating the engine invocation to compute all migrated rule groups at once.

## Rollback

```powershell
# if uncommitted
git checkout -- ApocalypticFastFood\ApocalypticFastFood\DiscountRules.cs
git checkout -- ApocalypticFastFood\ApocalypticFastFood\DiscountManager.cs
rm ApocalypticFastFood\ApocalypticFastFood.Tests\DiscountManagerFamilyIntegrationTests.cs
rm PATCH_10_DOCUMENTATION.md
```

## Commit-Message
`Refactor: add FamilyRule and wire into CalculateDiscount; add integration test and docs`
