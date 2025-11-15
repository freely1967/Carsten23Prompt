# Patch 6 — Wire VisitCountRule into `DiscountManager.CalculateDiscount`

Datum: 2025-11-15
Autor: (automatisch) Refactoring-Agent

## Kurzbeschreibung

Dieser Patch verlagert die visit-basierten Rabattberechnungen in die rule-basierte Engine. `VisitCountRule` wird ausgeführt und das Ergebnis (`visitDiscount`) wird an das Ende von `CalculateDiscount()` addiert. Die alte visit-count switch-Logik wird nicht sofort entfernt but its result variable (`s5`) is zeroed when `VisitCount > 0` to avoid double-counting.

## Änderungen
- In `DiscountManager.CalculateDiscount()` wird `visitDiscount` über `new DiscountEngine(new [] { new VisitCountRule() }).Calculate(this)` berechnet.
- Die Legacy-`switch (VisitCount)`-Bloecke werden weiterhin im Code (temporär) existieren but their contribution is suppressed by setting `s5 = 0.0` when VisitCount > 0.
- `visitDiscount` is added to the final `discount` together with `promoDiscount`.

## Tests
- `DiscountManagerVisitIntegrationTests.CalculateDiscount_Uses_VisitEngine_WhenVisitCountPresent` verifies that for a representative VIP scenario the `CalculateDiscount()` returns the same value as the `DiscountEngine` calculating the `VisitCountRule`.

## Rollback

```powershell
# if uncommitted
git checkout -- ApocalypticFastFood\ApocalypticFastFood\DiscountManager.cs
rm ApocalypticFastFood\ApocalypticFastFood.Tests\DiscountManagerVisitIntegrationTests.cs
rm PATCH_6_DOCUMENTATION.md
```

## Risk & Migration Notes
- Risk: Low-Medium. Additive and guarded; legacy logic is suppressed only when `VisitCount > 0` and replaced by the engine computation for that aspect.
- Migration plan: Continue migrating rule groups incrementally. For rules with complex side-effects (e.g. multiplier changes), ensure the rule replicates those multiplier modifications or adapt the engine to return both discount and multiplier adjustments.

## Commit-Message
`Refactor: wire VisitCountRule into CalculateDiscount; add integration test and docs`
