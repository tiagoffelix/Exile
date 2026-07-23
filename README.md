# Exile

Exile is a solo Unity project combining 3D daytime resource gathering and settlement progression with 2D nighttime combat.

## Solo Project

Role: Solo Programmer and Designer

I implemented the gameplay loop, progression, persistence, combat, shops, upgrades, difficulty modes, and scene flow in Unity and C#.

## Gameplay Loop

Day: gather wood, rock, and metal, then spend resources on buildings and upgrades.

Night: use that preparation in a 2D combat phase. Surviving advances the persistent run and returns the player to the next preparation phase.

## Economy and Progression

Prices needed to remain achievable across difficulty modes. Each mode also needed a reasonable number of days and nights, so daytime gathering could support preparation without removing pressure from the combat phase. Player statistics, resources, difficulty, and building state persist across scenes and sessions.

## Difficulty and Pacing

Difficulty modes increase combat pressure and reduce the margin for inefficient preparation. The daytime economy and upgrade prices were designed so increasing difficulty would not make progression impossible.

## Design Limitation

The primary scaling rule adds one enemy each night. This is a simple difficulty model. The rising count still increases pressure and encourages more efficient resource gathering during the day, but a richer curve could vary enemy composition and pressure more deliberately.

## Main Systems

- 3D movement, gathering, resource drops, and interactable world objects
- Settlement buildings, shops, upgrades, and affordability rules
- 2D nighttime combat and enemy spawning
- ScriptableObject-backed configuration and reusable interaction interfaces
- Persistent statistics, resources, difficulty, and building transforms
- Menus, settings, audio, and scene transitions

## Running the Project

Clone the repository and open the Unity project folder in the Unity version recorded by its project settings. The source is provided as a portfolio implementation and has no tagged release in this repository.

## Playable Build

[Play Exile on Itch.io](https://speazyy.itch.io/exile)

## Portfolio Case Study

[View the Exile case study](https://tiagoffelix.com/projects/exile/)
