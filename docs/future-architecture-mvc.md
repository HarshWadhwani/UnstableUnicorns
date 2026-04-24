# Future Architecture: MVC / Model Separation

**Context:** Captured from design discussion on 2026-04-23. Revisit when multiplayer, AI, or save/load becomes a priority.

---

## Current Approach

Game state lives directly in MonoBehaviours (`CardSpace.spaceCards`, `TurnManager.activePlayer`, etc.). The Unity hierarchy is a derived view synced to `spaceCards` — never read back for logic decisions. This is the enforced discipline that keeps the current approach workable.

## Why This Becomes a Problem for Multiplayer

Multiplayer (and AI, save/load, replays) requires game state that exists independently of the Unity scene:

- A **network layer** needs to serialize and send game state — you can't send a MonoBehaviour over the wire
- An **AI opponent** needs to simulate "what if I play this card" without actually mutating the scene
- **Save/load** needs to snapshot and restore state without reconstructing the entire GameObject tree
- **Replays** need to replay a sequence of state transitions deterministically

All of these become straightforward if game state is plain C# objects. They range from difficult to impossible if state only lives in scene components.

## The Target Architecture

### Model (pure C#, no Unity dependencies)
- `GameState` — owns everything: player list, active player, current phase, deck, discard pile
- `PlayerState` — hand, unicorn stable contents, upgrade/downgrade stables
- `DeckState` — ordered list of card definitions
- All game logic (shuffle, draw, play, win condition) operates on these objects
- Fully serializable — can be snapshotted, sent over a network, or saved to disk

### View (MonoBehaviours)
- One `CardSpaceView` per `CardSpace` — renders whatever the model says
- Receives a model reference or a diff and rebuilds its display
- No game logic — only layout and animation

### Controller
- Handles click input, validates against current model state, calls model mutations
- On model change, notifies affected views to re-render

## Migration Path (when the time comes)

Rather than a full rewrite, the migration can be incremental:

1. **Extract `GameState`** — move `TurnManager`'s player list, active player, and current phase into a plain C# class. `TurnManager` becomes a thin wrapper.
2. **Extract deck/hand state** — `spaceCards` moves into model objects; `CardSpace` MonoBehaviours become views that render from the model.
3. **Wire a render pass** — after any state mutation, call a `Render()` method that syncs views to model. Replace the scattered `PositionCardsInStable()` calls.
4. **Add serialization** — once state is plain C#, JSON serialization (or Unity's `JsonUtility`) gives you save/load and network transport for free.

## Trigger to Revisit

Start this work when any of these become real requirements:
- AI opponent that needs to evaluate hypothetical game states
- Network multiplayer (even just local LAN)
- Save/load game state
- Unit testing game logic without running the Unity scene
