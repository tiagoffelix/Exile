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

## Running the Project

Clone the repository and open the Unity project folder in the Unity version recorded by its project settings. The repository has no tagged release.

## Links

- [Play on itch.io](https://speazyy.itch.io/exile)
- [Case Study](https://tiagoffelix.com/projects/exile/)
