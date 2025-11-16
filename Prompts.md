Prompt: 1:
Ich will das du mir hilfst einen plan zu erstellen um folgende Spezifikation absolut zufriedenstellen zu absolvieren und alle punkte über ihrem ausmaß zu erledigen. Ich will das du sehr gründlich bist und nichts erfindest und mir einen sehr guten plan gibt um das ziel zu erreichen. Ich habe Gemini zur Verfügung sowie Copilot in vsc. Hilf mir einen exzellenten plan zu erstellen. 




Prompt: 2:
SYSTEM: Du bist ein erfahrenes C#-Architektur-Assistenzmodell, spezialisiert auf SOLID-Analyse.
USER:
Ich gebe dir das VS Projekt in dem Ordner ApocalypticFastFood. Analysiere die Dateien und liefere eine strukturierte Liste von SOLID-Verstößen.
Antworte ausschließlich als JSON-Liste mit Objekten:
[
  {
    "file": "<relative path>",
    "type": "<SingleResponsibility|OpenClosed|Liskov|InterfaceSeg|DependencyInversion>",
    "violation_summary": "<eine kurze Zusammenfassung, max 2 Sätze>",
    "root_cause": "<technische Ursache>",
    "evidence": ["<Codezeile oder Methodennamen als Beleg>"],
    "severity": "<High|Medium|Low>",
    "priority": <1-5>
  }
]
Führe keine Änderungen aus. Wenn du etwas nicht sicher bist, markiere field "confidence": "<low|medium|high>".
Du kannst den Ordner mit den Json dokumenten, achte darauf, dass du sorgfältig arbeitest und alle Codes und Unterordner durchgehst und alles einzeln Schritt für Schritt anschaust, ohne etwas zu überspringen oder erfinden. Gebe dir sehr viel mühe und erfülle die gegebene Aufgabe bis zum Schluss. 



Prompt: 3:
Bitte prüfe gründlich, ob die folgenden C#-Dateien vollständig überprüft worden Sind nach den SOLID verstößen und auch alle weiteren C#-Dateien in allen Unterordnern. Gehe jede Datei bis zum Ende durch (nicht nur die ersten Zeilen) und melde fehlende oder unvollständige Dateien sowie offensichtliche Syntaxfehler oder abgeschnittenen Code.

Zu überprüfende Dateien:

Customer.cs
DiscountManager.cs
MinorCustomer.cs
Program.cs
VIPCustomer.cs



Prompt: 4:
SYSTEM: Du bist ein Senior C# Refactoring-Coach.
USER:
Gegeben ist die Analyse solid_violations.json . Für jeden Eintrag erzeuge:

Eine prägnante Refactoring-Strategie (Pattern, z. B. Strategy/Factory/Adapter).
Konkrete, schrittweise Implementations-Schritte (nummeriert).
Ein unified diff Beispiel (nur die relevantesten Dateien, 15-50 Zeilen).
Unit-Testfälle, die zeigen, dass Funktionalität erhalten bleibt.
Gib das Ergebnis als JSON mit keys: "strategy", "steps", "diff", "tests", "risk".
Arbeite Sorgfältig und erstelle eine neue Datei wo du dein Ergebniss abspeicherst, erfinde nichts und gehe alle einträge durch.








Prompt: 5:
Erstelle mir hierraus einen prompt: 
Fertig — ich habe die Refactoring-Pläne erstellt und in refactor_plans_v1.json abgelegt. Kurzübersicht: Datei: refactor_plans_v1.json Inhalt: Für jeden SOLID-Eintrag aus deiner Analyse gibt es: eine prägnante Strategie (Pattern-Empfehlung), nummerierte Implementierungs-Schritte, ein kurzes unified-diff-Beispiel (relevanteste Dateien, kompakt gehalten), Beispiel-Unit-Test-Snippets (xUnit-stil) zur Verifikation, eine kurze Risiko-Einschätzung. Das ist was ich als Antwort bekommen habe und ich will einen Prompt erstellen der das hier macht: Ich kann die Änderungen schrittweise per Patches (apply_patch) implementieren, beginnend mit den geringsten Risiken (z. B. PriceCatalog, Payment interface segregation). achte dabei das der Promt sehr schön strukturiert ist und auch sorgfältig alle wichtigen sachen abdeckt an die man denken müsste. Hier ist ein abschnitt der JSON: [ { "file": "ApocalypticFastFood/ApocalypticFastFood/VIPCustomer.cs", "strategy": "Extract discount behavior to an IDiscountStrategy and use composition (Strategy Pattern).", "steps": [ "1) Add IDiscountStrategy with GetDiscount(CustomerContext).", "2) Implement VipDiscountStrategy.", "3) Make Customer delegate GetDiscount to an injected IDiscountStrategy (constructor injection).", "4) VipCustomer composes VipDiscountStrategy instead of overriding and throwing.", "5) Add tests confirming VIP path returns a numeric discount and does not throw." ], "diff": "--- a/Customer.cs\n+++ b/Customer.cs\n@@\n-public class Customer { public virtual double GetDiscount() { return 0; } }\n+public interface IDiscountStrategy { double GetDiscount(CustomerContext ctx); }\n+public class Customer { private readonly IDiscountStrategy _d; public Customer(IDiscountStrategy d) { _d = d; } public double GetDiscount(CustomerContext ctx) => _d.GetDiscount(ctx); }", "tests": "[Fact] public void VipCustomer_GetDiscount_DoesNotThrow() { var vip = new VipCustomer(); var ctx = new CustomerContext(); var d = vip.GetDiscount(ctx); Assert.IsType<double>(d); }", "risk": "Medium - changes constructor surface; provide adapters for transition." }


daruas folgt Prompt 5.1: 

Du findest im Repository die Datei refactor_plans_v1.json. Sie enthält für jedes gefundene SOLID-Problem die Felder file, strategy, steps, diff, tests, risk.
Deine Aufgabe ist: Alle Refactorings aus dieser JSON vollständig und schrittweise per apply_patch umzusetzen – beginnend mit den Einträgen mit dem geringsten Risiko.

Regeln

Sortiere alle Einträge strikt nach Risiko: Low → Medium → High.

Für jeden Eintrag erzeugst du einen vollständigen Refactoring-Block inklusive:

kurze Beschreibung

präzise Schritt-Liste (aus steps, sinnvoll ergänzt)

vollständig anwendbarer unified-diff Patch für apply_patch

zugehörige Unit-Tests (xUnit-Stil)

Risiko- & Migrationshinweise

Rollback-Hinweis

kurze Commit-Message

Die Patches müssen syntaktisch korrekt, kompilierbar und vollständig sein.

Erweitere das JSON-diff zu vollständigen Patches (inkl. Imports, Namespaces, Constructors), aber keine Platzhalter, kein Pseudocode.

Bei API-Breaking Changes erzeugst du automatisch Adapter / Stubs / Übergangs-APIs, damit der Build nicht bricht.

Medium-Risiko-Patches: zusätzlich Revert-Plan + Kompatibilitätsnotizen.

High-Risiko-Patches: statt direkter Anwendung → Review-Patch + klarer Migrationsplan + Liste offener Punkte.

Nach jedem Patch: Testcode integrieren (aus tests, sinnvoll ergänzt).

Am Ende:

Reihenfolge aller Patches

Gesamt-Checkliste (Build, Tests, Migration)

empfohlene Folge-Patches

kritische Stellen markieren

Ausgabeformat

Für jedes Refactoring erzeugst du exakt diesen Block:

### Patch <N>: <file> — <strategy> (Risk: <Low|Medium|High>)

1) Kurzbeschreibung
   - <1–2 Sätze>

2) Schritte
   - 1. …
   - 2. …
   - …

3) apply_patch
--- a/<Pfad>
+++ b/<Pfad>
@@
-<alte Zeile>
+<neue Zeile>

[weitere hunks falls mehrere Dateien betroffen sind]

4) Tests (xUnit)
```csharp
[Fact]
public void <Name>() {
    // Arrange
    ...
    // Act
    ...
    // Assert
    ...
}


Risiko & Migration

<konkrete Hinweise>

Rollback

<Wie revertbar?>

Commit-Message

<ein kurzer Vorschlag>

## Ablauf

- Lies `refactor_plans_v1.json` ein.  
- Sortiere nach Risiko.  
- Erzeuge den ersten Patch-Block vollständig.  
- Danach Patch 2, Patch 3, …  
- Am Ende: Zusammenfassung + Checkliste.

Beginne jetzt.


 
 dazu kommt 
 Prompt: 5.2:
 ja erstelle aber auch ein dokument wo du genau dokumentioerst was du gemacht hast also paatch 1 was war und was wurde gemacht,

da der prompt sehr groß war wurde imme riweder gefragt ob man so wwiter machen will woraufhin man mit 
Prompt: 5.3:
"Bitte fahre fort.
Achte dabei gründlich auf folgende Punkte:

Sorgfältigkeit – arbeite präzise, strukturiert und vollständig.

Nichts auslassen – alle Inhalte, Details und Schritte vollständig berücksichtigen.

Nichts erfinden – keinerlei Informationen hinzufügen, die nicht im ursprünglichen Text stehen.

Konsistenz – Stil, Struktur und Inhalt durchgehend beibehalten.

Klarheit – verständlich, korrekt und gut lesbar formulieren.

Falls Unklarheiten bestehen oder Informationen fehlen, bitte zuerst nachfragen, anstatt Annahmen zu treffen."






Prompt: 6:
Are all of the refactor plans done ?


Prompt: 7:
Cleanup the CashPaymentProcessor first then Implement the VIPCustomer → IDiscountStrategy refactor (JSON marked Medium). This can be done incrementally with an adapter so constructors remain compatible:
Add IDiscountStrategy and VipDiscountStrategy.
Add an adapter so Customer keeps a default strategy if none provided.
Add xUnit tests that assert VIP path returns numeric discounts and does not throw.
Commit, push, run tests.



Prompt: 8:
AUS:
Das habe ich bekommen:
 What I did Moved DiscountManagerDiscountProvider into Adapters/ and removed the original file at repository root. Added CustomerContracts.cs containing the interface contracts that were removed during the Customer conversion. Added Adapters/CustomerDiscountProvider.cs to preserve legacy behavior by mapping a data-only Customer into DiscountManager. Added a compatibility constructor to Customer so existing subclass constructors (e.g., VipCustomer) continue compiling. Updated PATCH_21_REVIEW_CUSTOMER_REFACTOR.md to reflect that the final conversion was applied and to document the compatibility artifacts and remaining follow-ups. Ran dotnet test ApocalypticFastFood.sln after changes — restore/build succeeded. Files added/changed Added: CustomerContracts.cs Added: CustomerDiscountProvider.cs Added: DiscountManagerDiscountProvider.cs Updated: Customer.cs (compat constructor) Deleted: DiscountManagerDiscountProvider.cs (original) Updated: PATCH_21_REVIEW_CUSTOMER_REFACTOR.md Why this order The contract file and adapters allow the final Customer conversion to be applied with minimal breaking changes to other code and tests. Centralizing adapters under Adapters/ improves discoverability and prepares the codebase for the next cleanup steps. What remains (recommended next steps) Run full test report (xUnit results) to confirm no runtime test failures: I can run and capture full test results (counts and failures) if you want. Review and confirm domain behavior: VIP discount rules (VipDiscountStrategy) — currently conservative (returns 0.0). VipLoyaltyCalculator — returns -1 (legacy oddity); confirm expected multiplier. Replace remaining direct usages of Customer behavior across code/tests with service interfaces (IDiscountProvider, IPurchaseService, ILoyaltyPointsCalculator) incrementally and remove compatibility shims after migration. Open PR and request domain reviewers, include PATCH_21_REVIEW_CUSTOMER_REFACTOR.md and PR_READY.md in the PR body. What I can do next (pick one) A) Run tests with detailed xUnit output and upload the results. B) Replace remaining legacy adapter locations and ensure all adapters are under Adapters/ (I already moved DiscountManager adapter; I can scan and centralize any others). C) Prepare the PR body and checklist and (if you prefer) attempt to create the PR (note: gh may not be installed in this environment). D) Start converting call sites to use IDiscountProvider / IPurchaseService and remove one compatibility shim (low-risk incremental). Tell me which of A/B/C/D you'd like next, or specify another task. 
 Gebe mir einen plaintext prompt damit a und b schön sorgfältig erledigt werden.



FOLG der Prompt 8.1:
Task A — Full xUnit Test Execution (detailed output)
Führe alle Tests im gesamten Repository aus und liefere folgendes vollständig und strukturiert:

Gesamtergebnis des Testlaufs

Anzahl aller Tests

Anzahl der bestandenen Tests

Anzahl der übersprungenen Tests

Anzahl der fehlgeschlagenen Tests

Vollständiges xUnit-Output-Log

inkl. aller Fehlermeldungen

inkl. Stacktraces

inkl. Testnamen und Klassen

Zusammenfassung aller Fehler

nach Testklasse gruppiert

inklusive kurzer Interpretation, was der Fehler bedeutet

keine Vermutungen, nur das, was aus dem Output ableitbar ist

Wichtig:

Nichts auslassen.

Kein Output beschneiden.

Keine Inhalte verändern oder umformulieren.

Reihenfolge beibehalten wie vom Test-Runner erzeugt.

Task B — Adapter-Scan + Konsolidierung
Scanne das gesamte Projekt sehr gründlich und führe folgende Schritte aus:

Identifiziere alle Adapter-ähnlichen Klassen

Klassen, die zwischen zwei Interfaces vermitteln

Klassen, die Legacy-Verhalten kapseln

Klassen, die Data-Modelle auf Domain-Modelle abbilden

Klassen mit “Provider”, “Adapter”, oder ähnlichen Namen

Auch implizite Adapter beachten (z. B. „helper“ Klassen, die Funktionalität überbrücken)

Liste alle gefundenen Adapter auf

mit vollständigem Pfad

mit kurzer 1-Satz-Beschreibung der Funktion

Prüfe, ob sie unter Adapters/ liegen

Falls nein: exakt angeben, wo sie aktuell liegen

Einschätzung: „move required“ oder „already correct“

Erstelle eine konkrete Move-Plan-Liste

Jede Datei einzeln aufführen

Zielordner angeben

kurz begründen, warum die Datei verschoben werden sollte oder nicht

Keine tatsächlichen Codeänderungen, nur Analyse + Plan

Abschließende Empfehlungen

potentielle Konflikte

Abhängigkeiten, die beim Verschieben beachtet werden müssen

Hinweise zu Namespaces und Usings, die betroffen sein könnten

Wichtig:

Keine Änderungen durchführen, nur exakt dokumentieren.

Keine Vermutungen über Funktionalität — nur das beschreiben, was im Code klar erkennbar ist.

Keine weiteren Schritte oder Aufgaben vorschlagen. Nur Task A + Task B.