# Animal Tab — Unofficial Update

Unofficial maintenance fork of [Animal Tab by Fluffy](https://github.com/fluffy-mods/AnimalTab), maintained by **Noslon** ([Warnu on GitHub](https://github.com/Warnu)). This is a full replacement, not an add-on, and is not endorsed by the original author.

## Status and scope

Fork release version: **1.7.0**. Supported game version: **RimWorld 1.7**. The internal assembly identity remains `AnimalTab, Version=8.0.0.0` to preserve the original binary identity; it is separate from the fork's release version.

**Release candidate for RimWorld 1.7.** Tested by Noslon on RimWorld 1.7.4981 rev136 with the Progression modpack and approximately 1,500 active mods: the Animals tab scrolled normally and displayed milk percentages. Testing used the patched DLL in the original mod's installation.

The build targets .NET Framework 4.7.2 using RimWorld 1.6.4871 reference assemblies. Only 1.7 is declared in the metadata; this is not a blanket compatibility claim. Development of this release is on branch `1.7`, based on the original mod's `1.5` branch.

The patch contains reflected null-reference failures in milk/wool status getters, leaving only the affected cell blank. It identifies the animal in one warning per component and retries after one second. Healthy values and the game's eligibility rules are preserved. Other exception types still propagate.

## Install

1. Install the current [Harmony mod](https://steamcommunity.com/sharedfiles/filedetails/?id=2009463077) for your game version.
2. Unsubscribe from the original **Animal Tab** and remove other local copies before extracting the release ZIP's `AnimalTab-Unofficial` folder into RimWorld's local `Mods` directory.
3. Enable this fork after Harmony. Both versions use the same package ID; keep only one installed.
4. Restart and test on a copy of your save.

Package ID: `Fluffy.AnimalTab`, preserved from the original for dependency checks and load-order rules. The fork uses its own Steam Workshop item ID, separate from the package ID.

## Build and test

Install the [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0) and PowerShell 7. Run from the repository root:

```powershell
./build.ps1
./package.ps1
```

The first build restores public NuGet dependencies. It requires no installed game, Steam account, machine-specific path, or `mod update` utility. GitHub Actions runs the same steps on Windows.

The regression harness tests the compiled DLL: healthy/inactive/missing components, the reflected null-reference failure, repeated redraws, one-second retries, recovery, independent components, clock rollover, and preservation of other exceptions. These tests do not boot RimWorld or establish mod-list compatibility.

The ZIP and staging folder appear under `dist/`. Packaging uses an explicit allowlist: assets, metadata, AnimalTab.dll, documentation, licenses, and checksums. No source tools, debug files, Harmony DLL, old update manifest, or Workshop ID is uploaded.

## Publishing and credits

See [docs/PUBLISHING.md](docs/PUBLISHING.md), [Workshop description](docs/WORKSHOP-DESCRIPTION.txt), and [CHANGELOG.md](CHANGELOG.md).

Original code/documentation: Fluffy and contributors, MIT. Original content: generally CC BY-SA 4.0, with third-party exceptions including BY-NC artwork/icons. Preserve [LICENSE](LICENSE) and [CREDITS.md](CREDITS.md). The original README is retained in [docs/UPSTREAM-README.md](docs/UPSTREAM-README.md), and the Git history is preserved.

Report fork issues at [Warnu/AnimalTab](https://github.com/Warnu/AnimalTab/issues) with your game version, mod list, and log from the failing session.
