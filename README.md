# Exile

A Unity project focused on **modular gameplay systems**, combining combat, resource gathering, base progression, and persistent world state.

Exile was built as a systems-driven prototype rather than a one-off gameplay scene. The project explores how combat, interaction, economy, and progression can work together inside a reusable gameplay framework.

## What I Built

- **Player movement and melee combat**
  - Implemented third-person character movement, attack timing, hit detection, and animation-driven combat flow.
  - Added support systems such as audio feedback, attack cooldowns, and contextual resource hit reactions.

- **Interaction framework**
  - Built an `IInteractable`-based interaction system using raycasts, allowing gameplay objects to expose interactions through a shared contract.
  - Used this to support gameplay features like mining, shops, and world interactions in a reusable way.

- **Resource and harvesting systems**
  - Created destructible resource nodes with health, drop logic, UI health feedback, and world-state updates.
  - Connected gathering directly to player progression by feeding materials into building and upgrade systems.

- **Economy and base progression**
  - Implemented building purchase flow with affordability checks, UI state updates, and unlock progression.
  - Added building data structures for purchase states, prices, and spawned placement data.

- **Persistent game state**
  - Built centralized save/load handling through a `GameManager`, persisting player stats, progression flags, world state, time-of-day state, and building transforms.
  - Used `ScriptableObject`s to separate configurable shared state from scene logic.

- **World state and scene flow**
  - Added ambient spawning, material spawning, time-of-day tracking, and end-of-day scene transitions.
  - Structured systems so progression state carries across sessions and scene changes.

## Technical Highlights

### Gameplay Systems
- Character movement and combat flow
- Resource harvesting and drop handling
- Building unlock and upgrade progression
- Interactable world objects

### Architecture
- Centralized `GameManager` for persistence and reset flow
- Interface-driven interaction via `IInteractable`
- Shared state through `ScriptableObject`s
- Separation between player systems, world systems, and progression systems

### Tools / Data Flow
- Building prices stored as reusable data structures
- Persistent building positions and rotations
- Spawn systems driven by tracked world counts
- Modular scene-to-scene state handoff

## Repository Structure

```text
Assets/
  Scripts/
    3D/           Core gameplay systems
    2D & Menu/    UI / menu / secondary scene scripts
  Scenes/
  Prefabs/
  Animations/
  Sounds/
  Images/
