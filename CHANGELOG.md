# Changelog

All notable changes to this project will be documented here. Versions are tagged around finished feature milestones.

---

## [v0.2.1] — 2026-04-23

### Bugfixes
- **B1:** `DeckManager.ShuffleDeck` now shuffles the `spaceCards` list directly (Fisher-Yates) and syncs the hierarchy to match — previously only the UI hierarchy was reordered, leaving draw order unaffected
- **B2:** `Stable.PositionCardsInStable` replaced mutable loop accumulator with direct index formula (`startX + i * cardSlotWidth`) — each card's position is now computed independently, removing a fragile ordering dependency

---

## [v0.2.0] — 2026-04-23

### CardActionExecutor System
- Introduced `CardActionExecutor` singleton with a queue-based action pipeline
- Cards define a `List<CardAction>` in their `CardData` — actions execute sequentially, pausing between steps to wait for player input
- Three action types implemented: `DiscardCardAction`, `GiveCardAction`, `DestroyCardAction`
- Stable click handlers (`HandStable`, `UnicornStable`, `UpgradeStable`, `DowngradeStable`) route clicks through the executor when a pending action is active

### Fuck Marry Kill Card
- First fully implemented magic card using the action system
- Effect sequence: opponent discards 1 card from hand → active player gives 1 card to opponent → active player destroys 1 opponent unicorn
- `MagicCardData` made abstract; `FuckMarryKillCardData` is the first concrete subclass

### Turn Phase Refactor
- `Special` phase split into two distinct phases:
  - `ImmediateSpecial` — triggered mid-turn when a card with `SpecialActionType.IMMEDIATE` is played; executor runs and resolves before advancing
  - `EveryTurnSpecial` — pre-draw phase that fires automatically at the start of each player's turn if they have `EVERY_TURN` cards in their unicorn, upgrade, or downgrade stables
- EVERY_TURN cards played from hand no longer incorrectly trigger the special phase
- `StartNextTurnPhase` now accepts the played card's `SpecialActionType` to determine phase routing

### Dynamic Card Loading
- `DeckManager` now loads all `CardData` assets from `Resources/CardDataInstances/` at runtime via `Resources.LoadAll`
- Removed the manually maintained `playCardDatas` list from the Inspector

---

## [v0.1.0] — 2026-04-23 (baseline)

### Core Game Loop
- Draw and play phases with turn switching between 2 players
- Active player draws from the play deck; card moves to hand
- Active player plays a card from hand; card routes to the correct destination based on `CardType`

### Card Routing
- `UNICORN` → active player's `UnicornStable`
- `UPGRADE` → active player's `UpgradeStable`
- `DOWNGRADE` → opponent's `DowngradeStable`
- `MAGIC` / `NEIGH` → `DiscardPile`

### Card Spaces
- `HandStable` with fan layout (hardcoded Y-offset arrays for 1–8 cards)
- `UnicornStable`, `UpgradeStable`, `DowngradeStable` with overlapping stack layouts
- `Deck` and `DiscardPile`

### Deck Setup
- Play deck built from `CardData` ScriptableObject instances loaded from Resources
- Baby unicorn deck dealt to each player's `UnicornStable` at game start
- Play deck shuffled at game start

### Win Condition
- `UnicornStable` checks card count against `maxCardsInStable`; logs on win (no game-over UI yet)
