# Patch 5 — Wire PromoCodeRule into `DiscountManager.CalculateDiscount`

Datum: 2025-11-15
Autor: (automatisch) Refactoring-Agent

## Kurzbeschreibung

Dieser Patch integriert die bereits implementierte `PromoCodeRule` in die bestehende `CalculateDiscount()`-Methode, ohne die gesamte Monolith-Logik sofort zu ersetzen. Promo-bezogene Logik wird aus dem monolithischen switch für `PromoCode` herausgenommen und stattdessen von `DiscountEngine` berechnet. Das Ergebnis (`promoDiscount`) wird anschließend zum Gesamt-Discount addiert.

Ziel: Schrittweise Migration der Monolith-Logik hin zu einer rule-basierten Engine mit minimalem Risiko und vollständiger Testabdeckung.

## Was wurde gemacht
- `CalculateDiscount()` berechnet jetzt `promoDiscount` via `new DiscountEngine(new [] { new PromoCodeRule() }).Calculate(this)`.
- Die bestehende `switch (PromoCode)` ist so angepasst, dass sie nur ausgeführt wird, wenn `PromoCode` leer ist; bei vorhandenem `PromoCode` wird `s13` auf `0` gesetzt (verhindert Double-Counting).
- Am Ende von `CalculateDiscount()` wird `promoDiscount` zur Summe hinzugefügt.

## Tests
- `DiscountManagerPromoIntegrationTests.CalculateDiscount_Uses_PromoEngine_WhenPromoPresent` prüft, dass bei vorhandenem Promo-Code `dm.CalculateDiscount()` den selben Wert liefert wie `DiscountEngine` mit `PromoCodeRule`.

## Schritte
1. Compute `promoDiscount` at the start of `CalculateDiscount()` using the `DiscountEngine` with only `PromoCodeRule`.
2. Guard the legacy `switch (PromoCode)` so it only runs when `PromoCode` is empty; otherwise set `s13 = 0`.
3. Add `promoDiscount` to final `discount` before returning.
4. Add integration test to verify parity in a representative case.

## Rollback

```powershell
# if uncommitted
git checkout -- ApocalypticFastFood\ApocalypticFastFood\DiscountManager.cs
rm ApocalypticFastFood\ApocalypticFastFood.Tests\DiscountManagerPromoIntegrationTests.cs
rm PATCH_5_DOCUMENTATION.md
```

## Risk & Migration Notes
- Risk: Low-Medium. Change is additive and guarded; core calculation remains unchanged for cases without `PromoCode`.
- Migration strategy: Continue moving rule groups (e.g., visit-count, birthday, time-based) into `DiscountEngine` and replace those sections in `CalculateDiscount()` incrementally.

## Commit-Message
`Refactor: wire PromoCodeRule into CalculateDiscount; add integration test and docs`
