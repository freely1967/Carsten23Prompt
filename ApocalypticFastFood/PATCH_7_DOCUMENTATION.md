# Patch 7 — Migrate Time-of-Day logic into `TimeOfDayRule` and wire into CalculateDiscount

Datum: 2025-11-15
Autor: (automatisch) Refactoring-Agent

## Kurzbeschreibung

Dieser Patch extrahiert die hour/time-of-day basierten Rabatt- und Straflogiken (u.a. die bisherigen `s2`, `s8`, `s17` Blöcke) in eine `TimeOfDayRule` und integriert diese Regel in die existing `DiscountEngine`. Die legacy-Teilwerte (`s2`, `s8`) werden innerhalb `CalculateDiscount()` auf `0` gesetzt, wenn die rule-basierten Werte berechnet werden, um Double-Counting zu vermeiden.

## Welche Dateien wurden geändert / neu hinzugefügt

- `DiscountRules.cs` — neue Klasse `TimeOfDayRule` hinzugefügt.
- `DiscountManager.cs` — berechnet `timeDiscount` via `DiscountEngine(new [] { new TimeOfDayRule() })`, setzt `s2` und `s8` auf `0` und addiert `timeDiscount` in die finale Summe.
- `ApocalypticFastFood.Tests/DiscountManagerTimeIntegrationTests.cs` — Integrationstest, der Parität für einen Nachmittagsszenario prüft.
- `PATCH_7_DOCUMENTATION.md` — dieses Dokument.

## Schritte
1. Implement `TimeOfDayRule` encapsulating afternoon/morning/night and rush-hour logic.
2. Compute `timeDiscount` via the rule engine early in `CalculateDiscount()`.
3. Zero-out legacy `s2` and `s8` values when the rule is applied to avoid double-counting.
4. Add `timeDiscount` to the final `discount`.
5. Add an integration test validating parity for a representative afternoon case.

## Tests
- `DiscountManagerTimeIntegrationTests.CalculateDiscount_Uses_TimeEngine_ForAfternoonCase` verifies the `CalculateDiscount()` uses the `TimeOfDayRule` result for a midday/afternoon case.

## Rollback

```powershell
# if uncommitted
git checkout -- ApocalypticFastFood\ApocalypticFastFood\DiscountRules.cs
git checkout -- ApocalypticFastFood\ApocalypticFastFood\DiscountManager.cs
rm ApocalypticFastFood\ApocalypticFastFood.Tests\DiscountManagerTimeIntegrationTests.cs
rm PATCH_7_DOCUMENTATION.md
```

## Risk & Migration Notes
- Risk: Medium. Time-based logic interacts with several other discount branches and multipliers; tests are essential for parity.
- Migration plan: Continue migrating other rule groups similarly and add tests for edge cases where multiple rules interact (e.g., rush-hour + promo + VIP multipliers).

## Commit-Message
`Refactor: add TimeOfDayRule and wire into CalculateDiscount; add integration test and docs`
