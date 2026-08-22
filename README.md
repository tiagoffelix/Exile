# Exile

Exile is a solo Unity/C# project with a persistent progression loop connecting 3D daytime preparation to 2D nighttime combat. Resources, upgrades, difficulty, and purchased buildings carry between the two phases.

## Role

Role: Solo Programmer and Designer

I implemented the gameplay loop, progression, persistence, combat, shops, upgrades, difficulty modes, and scene flow in Unity and C#.

## Gameplay Loop

Day: gather wood, rock, and metal, then spend resources on buildings and upgrades.

Night: use that preparation in a 2D combat phase. Surviving advances the persistent run and returns the player to the next preparation phase.

## Building and Upgrade Systems

Building costs are configured through `BuildingPositions` data. The shop checks affordability before offering purchases, while placeable buildings use a blueprint/ghost flow with collision and bounds validation, rotation, placement feedback, and stored transforms. Sword and armour upgrades use difficulty-dependent values.

## Shared State and Persistence

ScriptableObjects centralise selected shared state and configuration across scenes:

- `PlayerStats`: player statistics, resources, difficulty, and progression flags
- `TimeOfDayScript`: day/night timing state
- `BuildingPositions`: purchase state, building transforms, and building costs
- `NumberOfMaterials`: remaining tree and rock counts
- `Narrator`: first-day and first-night flags

`GameManager` saves and loads explicit values through PlayerPrefs. When the daytime scene loads, `AmbienceSpawner` uses that state to reconstruct purchased buildings and respawn the saved number of remaining resource nodes. Exact resource-node positions are not persisted. The remaining nodes are placed again within the configured spawn areas.

These ScriptableObjects hold the shared state needed across scenes. Other gameplay values still live in scene components or code, and some of the assets also carry mutable runtime state.

## Interaction System

The player casts a ray and invokes `IInteractable.Interact()` on supported targets. The shared contract covers the mine, the sword and armour upgrade shops, and the day-ending bed. Resource harvesting and blueprint placement use their own systems.

## Difficulty and Pacing

Difficulty changes enemy statistics and the size of sword and armour upgrades. Each completed night adds one enemy. Enemy composition itself does not change.

## Main Systems

- 3D movement, gathering, and resource drops
- Blueprint placement, settlement buildings, shops, upgrades, and affordability checks
- 2D nighttime combat and enemy spawning
- Raycast-based interaction for the mine, upgrade shops, and day-ending bed
- Shared ScriptableObject state plus PlayerPrefs persistence
- Purchased-building reconstruction and remaining resource-count restoration
- Menus, settings, audio, and scene transitions

## Controls

Every action reads through `GameInput`, so keyboard and mouse and the Android
on-screen controls drive the same gameplay code.

### Windows

| Action | Day (3D) | Night (2D) |
| --- | --- | --- |
| Move | W A S D | W A S D |
| Look | Mouse | — |
| Hold the camera still | Hold Right Mouse Button | — |
| Attack | Left Mouse Button | Left Mouse Button |
| Block | — | Hold Right Mouse Button |
| Interact with the mine, shops and bed | E | — |
| Open the shop | R | — |
| Place a building | Left Mouse Button | — |
| Pause | Esc | Esc |

### Android

| Action | Day (3D) | Night (2D) |
| --- | --- | --- |
| Move | Drag anywhere on the left of the screen | Same |
| Look | Drag on the right of the screen | — |
| Attack | Hit | Hit |
| Block | — | Hold Block |
| Interact | Use | — |
| Open the shop | Shop | — |
| Place a building | Place, shown only while a blueprint is active | — |
| Pause | Pause button, or the hardware back button | Same |

Building placement aims at the centre of the screen on touch, so the player
turns the camera to position a building and commits with Place. The on-screen
controls are built in code at runtime and appear only on mobile, so no scene or
prefab carries a mobile-only variant.

## Platforms and Testing

| Platform | Status | Verified |
| --- | --- | --- |
| Windows | Released on itch.io | Full loop played: menus, gathering, purchases, building, the transition to night, 2D combat, progression and saved state. |
| Android | Not published yet | Builds as a signed ARM64 APK (IL2CPP, Unity 2021.3.20f1). The touch layer compiles and installs; it has not been run on a physical device. |

## Running the Project

Clone the repository and open the `Exile` folder in Unity **2021.3.20f1**, the
version recorded in `ProjectSettings/ProjectVersion.txt`. The repository has no
tagged release.

### Building

Builds run through `Assets/Editor/BuildScript.cs`.

- `Build > Exile Windows` writes to `Builds/Windows`.
- `Build > Exile Android APK` writes `Builds/Android/Exile.apk`.

From the command line:

```bat
"<editor>\Unity.exe" -quit -batchmode -nographics -logFile - ^
  -projectPath "<repo>\Exile" -buildTarget Android ^
  -executeMethod BuildScript.BuildAndroid
```

The Android build needs Android Build Support with the Android SDK & NDK Tools
and OpenJDK modules. The build entry point sets IL2CPP with an ARM64 target,
landscape orientation and a non-development build at build time rather than
storing them in project settings, so opening the project does not quietly change
how the Windows build is produced.

**Signing.** No keystore is committed and none is configured, so Unity signs
with the local debug key. That produces an APK people can install directly from
itch.io after allowing installation from unknown sources. It is not a Google
Play release, and a Play submission would need a release keystore kept outside
this repository.

## Credits

Solo project. Third-party asset packs used, as they appear under
`Assets/Assets`: ASTROFISH_GAMES, Polytope Studio, PollyPrivateers, Medieval
Fortification, Medieval_Weapons, NatureStarterKit2, RawWoodenFurnitureFree, Rock
Package, Tree9 and Skybox for the 3D phase, a Mixamo character, and Hero Knight -
Pixel Art, Monsters Creatures Fantasy, Thaleah_PixelFont and ccadori's vector
forest scenery for the 2D phase. Some 2D art was produced with AI assistance, as
disclosed on the itch.io page. All gameplay code and systems are my own.

## Links

- [Download on itch.io](https://speazyy.itch.io/exile)
- [Case Study](https://tiagoffelix.com/projects/exile/)
