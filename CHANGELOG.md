# Changelog

All notable changes to this project will be documented here. Versions are tagged around finished feature milestones.

---

## [v0.2.2] — 2026-04-25

### Bugfixes
- **B4:** `DestroyCardAction.TargetStable` enum and per-stable targeting removed. The destroying player now picks any card from any of the opponent's three stables (unicorn/upgrade/downgrade) instead of being locked to a single stable. `CardActionExecutor.ExecutePendingAction` falls back to `card.cardSpace` as the source when `pendingSourceStable` is null, so the actual stable is derived from the clicked card. FMK's "Kill" step (previously `targetStable = Unicorn`) now allows targeting upgrades and downgrades as well.

### Refactors
- **R1:** Destroy-pending-action click guard moved from each stable subclass into `Stable.HandleCardClick`. `UnicornStable`, `UpgradeStable`, and `DowngradeStable` previously held three near-identical copies (Unicorn's had drifted with two stray `Debug.Log` lines). The leaf overrides are deleted; `HandStable` retains its own override since hand-click semantics differ.
- **R2:** Introduced abstract `StackedStable : Stable` to host the edge-stacked overlap layout shared by `UpgradeStable` and `DowngradeStable`. Subclasses now declare a `protected override bool FromRightEdge` (true for upgrade, false for downgrade) and inherit a `sealed` `PositionCardsInStable`. The duplicated RectTransform setup, overlap math, and a stale `// 20% overlap` comment are gone; the `sealed` modifier prevents future copy-paste drift.
- **R3:** Removed the duplicated `cardType`, `specialActionType`, and `afterAction` fields from `Card` (and the matching copy lines in `Initialize`). `CardData` is now the single source of truth — `CardManager.PlayCardForCurrentPlayer`, `TurnManager.CollectEveryTurnActionsFromSpace`, and `HandStable.HandleCardClick` now read through `card.cardData.*` directly. Eliminates the risk of the cached enums drifting from the underlying `CardData` asset and drops `afterAction` from `Card` entirely (it was never read; see R5).

---

## [v0.2.1] — 2026-04-23

### Bugfixes
- **B1:** `DeckManager.ShuffleDeck` now shuffles the `spaceCards` list directly (Fisher-Yates) and syncs the hierarchy to match — previously only the UI hierarchy was reordered, leaving draw order unaffected
- **B2:** `Stable.PositionCardsInStable` replaced mutable loop accumulator with direct index formula (`startX + i * cardSlotWidth`) — each card's position is now computed independently, removing a fragile ordering dependency
- **B3:** Removed no-op `TriggerSpecialAction` overrides from `BabyUnicornCardData`, `BasicNeighCardData`, `DiscardNeighCardData`, `FinalNeighCardData`, `UpgradeCardData`, and `DowngradeCardData` — these stubs suppressed the base class `actions` pipeline, silently preventing any future card effects from executing on these types

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
