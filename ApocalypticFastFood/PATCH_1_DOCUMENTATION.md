# Patch 1 — Centralize item pricing with IPriceCatalog

Datum: 2025-11-14
Autor: (automatisch) Refactoring-Agent

## Kurzbeschreibung

Dieser Patch extrahiert die hartkodierten Artikelpreise aus Inline-Logik und zentralisiert sie in einer einfachen Abstraktion `IPriceCatalog` mit einer In-Memory-Implementierung `InMemoryPriceCatalog`. Zusätzlich wurde `OrderProcessor` so verändert, dass es die Preise über das Katalog-Interface bezieht (statt einzelner if/else-Magic-Numbers). Ein kleines xUnit-Testprojekt mit einem Test für `InMemoryPriceCatalog` wurde erstellt.

Ziel: Reduzieren von Duplikation und Magic-Numbers, Vorbereitung für weitere Refactorings (z. B. Auslagern der Preisdaten in Konfiguration oder DB).

## Welche Dateien wurden geändert / neu hinzugefügt

Geänderte Datei:
- `ApocalypticFastFood/ApocalypticFastFood/DiscountManager.cs`
  - Ergänzt: `IPriceCatalog` und `InMemoryPriceCatalog` Typen am Ende der Datei
  - Geändert: `OrderProcessor` — Umstellung auf constructor injection, interne readonly-Felder, Nutzung von `_priceCatalog.GetPrice(item)`

Neue Dateien (Tests):
- `ApocalypticFastFood/ApocalypticFastFood.Tests/ApocalypticFastFood.Tests.csproj` (Projektdatei für xUnit)
- `ApocalypticFastFood/ApocalypticFastFood.Tests/PriceCatalogTests.cs` (Unit-Test für `InMemoryPriceCatalog`)

> Hinweis: Es wurde bewusst in `DiscountManager.cs` (gleicher Namespace) implementiert, um minimale Datei-Invasion zu haben. Langfristig sollten die neuen Typen in eigene Dateien/Ordner verschoben werden.

## Detaillierte Änderungen (Hauptpunkte)

1) `OrderProcessor`-Refactor
- `public DatabaseService DbSvc = new();` etc. wurden entfernt und durch private readonly Felder ersetzt:
  - `_dbSvc`, `_dm`, `_emailSvc`, `_smsSvc`, `_priceCatalog`
- Neuer Konstruktor `public OrderProcessor(IPriceCatalog? priceCatalog = null, DatabaseService? dbSvc = null, DiscountManager? dm = null, EmailService? emailSvc = null, SmsService? smsSvc = null)` der Standardimplementierungen injiziert, falls `null` übergeben.
- Preisberechnung in `ProcessOrder(...)` wurde geändert: statt einer langen if/else-Kette mit Magic-Numbers wird nun jede Position mit `_priceCatalog.GetPrice(item)` addiert.
- Restliche Logik (`CalculateDiscount`, `GenerateReceipt`, SaveOrder, Send Email`) bleibt unverändert funktional.

2) `IPriceCatalog` + `InMemoryPriceCatalog`
- `public interface IPriceCatalog { double GetPrice(string item); }`
- `public class InMemoryPriceCatalog : IPriceCatalog { ... }` mit Standardpreisen:
  - burger: 8.99
  - fries: 3.49
  - shake: 4.99
  - nuggets: 6.49
  - salad: 7.99
- `GetPrice` gibt `0.0` zurück, wenn Artikel unbekannt oder `null`.

3) Tests
- Erstellt ein neues Testprojekt `ApocalypticFastFood.Tests` (xUnit) mit ProjectReference auf das Hauptprojekt.
- Hinzugefügt `PriceCatalogTests.InMemoryPriceCatalog_ReturnsExpectedPrices` mit Assertions auf alle Standardartikel und `unknown`.

## Wichtige Code-Diffs (relevant, Auszug)

- `OrderProcessor` (vorher) — Magic numbers (gekürzt):
```diff
-    public DatabaseService DbSvc = new();
-    public DiscountManager Dm = new();
-    public EmailService EmailSvc = new();
-    public SmsService SmsSvc = new();
+    private readonly DatabaseService _dbSvc;
+    private readonly DiscountManager _dm;
+    private readonly EmailService _emailSvc;
+    private readonly SmsService _smsSvc;
+    private readonly IPriceCatalog _priceCatalog;
+
+    public OrderProcessor(IPriceCatalog? priceCatalog = null,
+                          DatabaseService? dbSvc = null,
+                          DiscountManager? dm = null,
+                          EmailService? emailSvc = null,
+                          SmsService? smsSvc = null)
+    {
+        _priceCatalog = priceCatalog ?? new InMemoryPriceCatalog();
+        _dbSvc = dbSvc ?? new DatabaseService();
+        _dm = dm ?? new DiscountManager();
+        _emailSvc = emailSvc ?? new EmailService();
+        _smsSvc = smsSvc ?? new SmsService();
+    }
@@
-    Dm.TotalAmount = 0;
-    foreach (var item in items)
-        if (item == "burger")
-        {
-            Dm.TotalAmount += 8.99;
-        }
-        else if (item == "fries")
-            Dm.TotalAmount += 3.49;
-        else if (item == "shake")
-            Dm.TotalAmount += 4.99;
-        else if (item == "nuggets")
-            Dm.TotalAmount += 6.49;
-        else if (item == "salad")
-            Dm.TotalAmount += 7.99;
+    _dm.TotalAmount = 0;
+    foreach (var item in items)
+    {
+        _dm.TotalAmount += _priceCatalog.GetPrice(item);
+    }
```

- `IPriceCatalog` / `InMemoryPriceCatalog` (neu, Auszug):
```csharp
public interface IPriceCatalog
{
    double GetPrice(string item);
}

public class InMemoryPriceCatalog : IPriceCatalog
{
    private readonly Dictionary<string, double> _prices = new()
    {
        ["burger"] = 8.99,
        ["fries"] = 3.49,
        ["shake"] = 4.99,
        ["nuggets"] = 6.49,
        ["salad"] = 7.99
    };

    public double GetPrice(string item)
    {
        if (item is null) return 0.0;
        return _prices.TryGetValue(item, out var p) ? p : 0.0;
    }
}
```

## Wie teste ich die Änderungen lokal

PowerShell / Windows (im Workspace-Root `c:\Users\elydu\Desktop\Carsten23Prompt\ApocalypticFastFood`):

```powershell
# 1) restore
dotnet restore

# 2) run tests (in Test-Projekt)
cd ApocalypticFastFood.Tests
dotnet test --logger:trx
```

Alternativ (ein Kommando von Root):

```powershell
dotnet test ApocalypticFastFood\ApocalypticFastFood.Tests\ApocalypticFastFood.Tests.csproj
```

Der Test `InMemoryPriceCatalog_ReturnsExpectedPrices` sollte grün sein.

## Rollback

Wenn du den Patch zurücknehmen willst:

```powershell
# Option A: Git Revert (sicher)
git revert <commit-hash-of-patch-1>

# Option B: falls noch nicht committed, einfach hard reset
git checkout -- ApocalypticFastFood\ApocalypticFastFood\DiscountManager.cs
rm -r ApocalypticFastFood\ApocalypticFastFood.Tests
```

Hinweis: Commit-Hashes siehst du mit `git log --oneline`.

## Migration / Kompatibilitäts-Hinweise

- `OrderProcessor` hat jetzt einen neuen Konstruktor mit optionalen Parametern. Bestehender Code, das `new OrderProcessor()` verwendet, bleibt weiterhin kompatibel, weil alle Konstruktor-Parameter optional sind.
- Künftige Refactorings sollten `DiscountManager.CalculateDiscount` und `GenerateReceipt` sukzessive so anpassen, dass sie die `IPriceCatalog` verwenden, damit eine einzige Quelle die Preise bestimmt.

## Commit-Message

`Refactor: Centralize item pricing (IPriceCatalog + InMemoryPriceCatalog); OrderProcessor uses catalog; add PriceCatalog unit tests`

## Nächste empfohlene Schritte

1. Segregate payment interfaces und entfernen von `NotImplementedException` in `CashPaymentProcessor` (Low Risk).
2. Extract `ReceiptFormatter` aus `DiscountManager.GenerateReceipt` (Low Risk).
3. Introduce `IEmailSender` / `ISmsSender` und inject them into `OrderProcessor` (Low-Medium Risk).
4. Decompose `DiscountManager.CalculateDiscount` rule-set in `IDiscountRule`s (Medium-High Risk) — umfangreich, zuerst Review-Patch + Tests.

---

Wenn du möchtest, setze ich sofort Patch 2 (Payment interface segregation) um. Soll ich fortfahren?