# Patch 4 — Introduce DiscountEngine and pluggable IDiscountRule implementations (initial low-risk rules)

Datum: 2025-11-15
Autor: (automatisch) Refactoring-Agent

## Kurzbeschreibung

Dieser Patch führt eine kleine `DiscountEngine` ein, die eine Liste von `IDiscountRule`-Implementierungen ausführt. Zwei sichere, low-risk Regeln wurden hinzugefügt: `PromoCodeRule` (handhabt PromoCodes wie `SAVE10`, `SAVE20`, `VIP50`, `STUDENT25`, `FREEFRIES`) und `VisitCountRule` (handhabt Rabatt-Bereiche basierend auf `VisitCount`). Ziel ist, die große `CalculateDiscount`-Logik in späteren Schritten regelweise zu extrahieren.

Wichtig: Diese Änderung ist nicht invasiv — die neue Engine ist bereit zur Nutzung, aber die vorhandene `CalculateDiscount()`-Monolith-Logik wird noch nicht vollständig ersetzt. Das erlaubt schrittweises, verifizierbares Migrieren ohne Verhaltensänderung.

## Dateien hinzugefügt
- `ApocalypticFastFood/ApocalypticFastFood/DiscountEngine.cs`
- `ApocalypticFastFood/ApocalypticFastFood/DiscountRules.cs`
- `ApocalypticFastFood/ApocalypticFastFood.Tests/DiscountEngineTests.cs`
- `ApocalypticFastFood/PATCH_4_DOCUMENTATION.md`

## Schritte (ausführlich)
1) Define `IDiscountRule` and `DiscountEngine`.
2) Implement `PromoCodeRule` with the promo code cases present in the original monolith. Add defensive checks for missing collections.
3) Implement `VisitCountRule` to cover visit-count-based logic replicated from `CalculateDiscount`.
4) Add unit tests for both rules via `DiscountEngineTests`.
5) Do NOT replace `CalculateDiscount()` yet; instead ensure the engine is available for incremental migration.
6) Document the change and next migration steps (see "Nächste Schritte").

## Tests
- `DiscountEngineTests.Engine_Applies_PromoCodeRule_SAVE10` validates that `SAVE10` with `TotalAmount=60` yields 10.0 from the engine.
- `DiscountEngineTests.Engine_Applies_VisitCountRule_100plus` validates a Diamond VIP path yields 45.0 from `VisitCountRule`.

## Migration notes & Next steps
- Next patch: wire `DiscountEngine` into `DiscountManager.CalculateDiscount` for a subset of rules only (e.g., apply engine results for promo/visit-rules and then call legacy logic for remaining rules). Include tests to ensure no double-counting and behavior parity.
- For complex rules, create Review-Patches where domain semantics are ambiguous before replacing them.

## Rollback

```powershell
# if uncommitted
rm ApocalypticFastFood\ApocalypticFastFood\DiscountEngine.cs
rm ApocalypticFastFood\ApocalypticFastFood\DiscountRules.cs
rm ApocalypticFastFood\ApocalypticFastFood.Tests\DiscountEngineTests.cs
git restore --staged .
```

## Commit-Message
`Refactor: add DiscountEngine + PromoCodeRule and VisitCountRule; add tests`

## Risk
- Low-Medium: safe because code is additive and not yet wired into main discount calculation. Future wiring must be done rule-by-rule with tests.
