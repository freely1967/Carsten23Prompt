1.
Ich will das du mir hilfst einen plan zu erstellen um folgende Spezifikation absolut zufriedenstellen zu absolvieren und alle punkte über ihrem ausmaß zu erledigen. Ich will das du sehr gründlich bist und nichts erfindest und mir einen sehr guten plan gibt um das ziel zu erreichen. Ich habe Gemini zur Verfügung sowie Copilot in vsc. Hilf mir einen exzellenten plan zu erstellen. 
"gasodgasdgaslhdgalskdglahsdgla"



2.
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



3.
Bitte prüfe gründlich, ob die folgenden C#-Dateien vollständig überprüft worden Sind nach den SOLID verstößen und auch alle weiteren C#-Dateien in allen Unterordnern. Gehe jede Datei bis zum Ende durch (nicht nur die ersten Zeilen) und melde fehlende oder unvollständige Dateien sowie offensichtliche Syntaxfehler oder abgeschnittenen Code.

Zu überprüfende Dateien:

Customer.cs
DiscountManager.cs
MinorCustomer.cs
Program.cs
VIPCustomer.cs



4.
SYSTEM: Du bist ein Senior C# Refactoring-Coach.
USER:
Gegeben ist die Analyse solid_violations.json . Für jeden Eintrag erzeuge:

Eine prägnante Refactoring-Strategie (Pattern, z. B. Strategy/Factory/Adapter).
Konkrete, schrittweise Implementations-Schritte (nummeriert).
Ein unified diff Beispiel (nur die relevantesten Dateien, 15-50 Zeilen).
Unit-Testfälle, die zeigen, dass Funktionalität erhalten bleibt.
Gib das Ergebnis als JSON mit keys: "strategy", "steps", "diff", "tests", "risk".
Arbeite Sorgfältig und erstelle eine neue Datei wo du dein Ergebniss abspeicherst, erfinde nichts und gehe alle einträge durch.








5.
Fertig — ich habe die Refactoring-Pläne erstellt und in refactor_plans_v1.json abgelegt. Kurzübersicht: Datei: refactor_plans_v1.json Inhalt: Für jeden SOLID-Eintrag aus deiner Analyse gibt es: eine prägnante Strategie (Pattern-Empfehlung), nummerierte Implementierungs-Schritte, ein kurzes unified-diff-Beispiel (relevanteste Dateien, kompakt gehalten), Beispiel-Unit-Test-Snippets (xUnit-stil) zur Verifikation, eine kurze Risiko-Einschätzung. Das ist was ich als Antwort bekommen habe und ich will einen Prompt erstellen der das hier macht: Ich kann die Änderungen schrittweise per Patches (apply_patch) implementieren, beginnend mit den geringsten Risiken (z. B. PriceCatalog, Payment interface segregation). achte dabei das der Promt sehr schön strukturiert ist und auch sorgfältig alle wichtigen sachen abdeckt an die man denken müsste. Hier ist ein abschnitt der JSON: [ { "file": "ApocalypticFastFood/ApocalypticFastFood/VIPCustomer.cs", "strategy": "Extract discount behavior to an IDiscountStrategy and use composition (Strategy Pattern).", "steps": [ "1) Add IDiscountStrategy with GetDiscount(CustomerContext).", "2) Implement VipDiscountStrategy.", "3) Make Customer delegate GetDiscount to an injected IDiscountStrategy (constructor injection).", "4) VipCustomer composes VipDiscountStrategy instead of overriding and throwing.", "5) Add tests confirming VIP path returns a numeric discount and does not throw." ], "diff": "--- a/Customer.cs\n+++ b/Customer.cs\n@@\n-public class Customer { public virtual double GetDiscount() { return 0; } }\n+public interface IDiscountStrategy { double GetDiscount(CustomerContext ctx); }\n+public class Customer { private readonly IDiscountStrategy _d; public Customer(IDiscountStrategy d) { _d = d; } public double GetDiscount(CustomerContext ctx) => _d.GetDiscount(ctx); }", "tests": "[Fact] public void VipCustomer_GetDiscount_DoesNotThrow() { var vip = new VipCustomer(); var ctx = new CustomerContext(); var d = vip.GetDiscount(ctx); Assert.IsType<double>(d); }", "risk": "Medium - changes constructor surface; provide adapters for transition." },


hierraus folgt : 

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
 ja erstelle aber auch ein dokument wo du genau dokumentioerst was du gemacht hast also paatch 1 was war und was wurde gemacht,
 