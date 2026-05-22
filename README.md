# Block Drift (minimal demo)

This is a minimal, self-contained C# WinForms demo for "Block Drift" — a voxel-styled drifting game prototype.

Build & run (requires .NET 7+ SDK):

```powershell
dotnet build "d:\Github\Block-Drift\Block-Drift.csproj"
dotnet run --project "d:\Github\Block-Drift\Block-Drift.csproj"
```

Controls:
- Arrow keys: Move
- Space: Boost
- D: Drop oil (demo)
- C: Swap with an NPC
- K: Skydive (jump)

Notes:
- This is a simplified prototype demonstrating voxels-like rendering, cars with rarities, power-ups, weather/time, and a simple home screen.
- Expand by adding assets, physics, audio, networking, and richer maps/events.
