# Patch 9 — Move `Program` demos to `Samples.SampleRunner` and simplify Program bootstrap

Datum: 2025-11-15
Autor: (automatisch) Refactoring-Agent

## Kurzbeschreibung

Dieser Patch verschiebt die demonstrativen Szenarien aus `Program.cs` in `Samples/SampleRunner.cs` (neue Datei) und vereinfacht `Program.cs` zu einem Bootstrapper, das Demo-Szenarien nur bei Bedarf ausführt (Argument `samples` oder Environment `RUN_SAMPLES=1`). Ein leichter Integrationstest stellt sicher, dass `SampleRunner.RunDemos()` keine Ausnahmen wirft.

## Dateien geändert / hinzugefügt
- `Program.cs` — reduziert, ruft `Samples.SampleRunner.RunDemos()` nur auf Anfrage.
- `Samples/SampleRunner.cs` — neue Datei mit die bisherigen Demo-Szenarien (Test 1..4 + LSP examples).
- `ApocalypticFastFood.Tests/SampleRunnerTests.cs` — test that calls `SampleRunner.RunDemos()` to verify demo runs without exceptions.
- `PATCH_9_DOCUMENTATION.md` — dieses Dokument.

## Warum
- Keeps `Program` minimal and focused on bootstrap.
- Makes demo code easier to test, run, or exclude from production builds.
- Prepares repository for CI usage where demos are runnable but opt-in.

## Wie benutzt man die Demos
PowerShell (Workspace root):

```powershell
# Run program normally (no demos)
dotnet run --project ApocalypticFastFood\ApocalypticFastFood.csproj

# Run demos via argument
dotnet run --project ApocalypticFastFood\ApocalypticFastFood.csproj -- samples

# Or via environment variable
$env:RUN_SAMPLES = "1"; dotnet run --project ApocalypticFastFood\ApocalypticFastFood.csproj
```

## Rollback

```powershell
# if uncommitted
git checkout -- ApocalypticFastFood\ApocalypticFastFood\Program.cs
rm -r ApocalypticFastFood\ApocalypticFastFood\Samples
rm ApocalypticFastFood\ApocalypticFastFood.Tests\SampleRunnerTests.cs
rm PATCH_9_DOCUMENTATION.md
```

## Commit-Message
`Refactor: move Program demos to Samples.SampleRunner; simplify Program bootstrap; add tests and docs`
