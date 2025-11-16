# Patch 2 — Refactor OrderProcessor to accept a payment processor

Datum: 2025-11-14
Autor: (automatisch) Refactoring-Agent

## Kurzbeschreibung

Dieser Patch verändert `OrderProcessor`, so dass Zahlungsabwicklung über ein injizierbares `IPaymentProcessor` erfolgt. Das ermöglicht das Ersetzen/Mocken der Zahlungslogik für Tests und das Einhalten des Interface Segregation Principle (ISP) durch Verwendung kleiner, fokussierter Payment-Interfaces (siehe Patch 1).

Ziel: Entfernen von direkten Zahlungs-Implementierungsabhängigkeiten aus `OrderProcessor`, Erleichterung von Tests und Vorbereitung für weitere Splits (z.B. echte Payment-SDK-Adapter).

## Welche Dateien wurden geändert / neu hinzugefügt

Geänderte Datei:
- `ApocalypticFastFood/ApocalypticFastFood/DiscountManager.cs`
  - `OrderProcessor` erhält ein neues Feld `_paymentProcessor` und einen optionalen Konstruktor-Parameter `IPaymentProcessor? paymentProcessor = null`.
  - `ProcessOrder` führt nach dem Speichern der Bestellung eine vereinfachte Zahlungsabwicklung aus, basierend auf `DiscountManager.PaymentMethod`.

Neue Testdatei:
- `ApocalypticFastFood/ApocalypticFastFood.Tests/OrderProcessorPaymentTests.cs`
  - Test `OrderProcessor_Uses_PaymentProcessor_ForCash` prüft, dass bei `PaymentMethodEnum = PaymentMethod.Cash` die `ProcessCash`-Methode des bereitgestellten `IPaymentProcessor` aufgerufen wird.

## Detaillierte Änderungen (Auszug)

- `OrderProcessor` Konstruktor:
  - Vorher: keine Abhängigkeit auf ein Payment-Interface.
  - Nachher: zusätzlicher optionaler Parameter `IPaymentProcessor? paymentProcessor = null`.
  - Default-Verhalten: wenn `null`, wird ein `PaymentProcessorFacade` mit `CashPaymentProcessor` und `CardPaymentProcessor` injiziert, damit bestehende Aufrufer `new OrderProcessor()` weiterhin funktionieren.

- `ProcessOrder`:
  - Nach dem `SaveOrder` wird ein `switch` auf `PaymentMethod` ausgeführt und der `IPaymentProcessor` zur Ausführung der Zahlungen verwendet.
  - Implementierte Fälle (vereinfachte Stubs): `cash`, `credit`, `debit`, `paypal`.

## Tests & Ausführung

PowerShell (im Workspace-Root `c:\Users\elydu\Desktop\Carsten23Prompt\ApocalypticFastFood`):

```powershell
# 1) restore
dotnet restore

# 2) run tests
dotnet test ApocalypticFastFood\ApocalypticFastFood.Tests\ApocalypticFastFood.Tests.csproj
```

Die Tests `Facade_Delegates_ProcessCash` und `OrderProcessor_Uses_PaymentProcessor_ForCash` sollten grün sein.

## Rollback

Um die Änderungen zurückzunehmen:

```powershell
# wenn committed
git revert <commit-hash>

# oder falls uncommitted
git checkout -- ApocalypticFastFood\ApocalypticFastFood\DiscountManager.cs
rm ApocalypticFastFood\ApocalypticFastFood.Tests\OrderProcessorPaymentTests.cs
```

## Migration / Kompatibilität

- Konstruktor-Parameter sind optional; bestehender Aufruf `new OrderProcessor()` bleibt kompatibel.
- Künftige Arbeit: Ersetzen der stub-Parameter (z.B. credit card number) durch echte Zahlungs-Adapter/DTOs und Sicherheits-/PCI-konforme Implementierungen.

## Nächste Schritte (empfohlen)

1. Implement a real `CardPaymentProcessor` adapter that wraps the payment SDK (medium risk).
2. Add dedicated tests for other payment methods and error flows (medium risk).
3. Consider splitting `PaymentProcessorFacade` into a factory that composes processors from DI container configuration.

---

Soll ich jetzt den `CardPaymentProcessor` weiter ausbauen (Patch 3), oder möchtest du, dass ich die Änderungen commite und einen PR-Text vorbereite?