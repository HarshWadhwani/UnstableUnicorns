# Changelog

All notable changes to this project will be documented here. Versions are tagged around finished feature milestones.

---

## [v0.2.6] — 2026-06-04

### Feature: EVERY_TURN phase mechanism
- `EveryTurnSpecial` phase is now fully functional. At the start of each player's turn, cards with `specialActionType = EVERY_TURN` in any of their stables are collected into `everyTurnCardsPending`.
- Player activates cards individually by clicking them in their stable during `EveryTurnSpecial`. Clicking a pending card calls `TurnManager.TryActivateEveryTurnCard()`, which removes it from the list and fires its action queue.
- `TurnManager.SkipEveryTurnPhase()` is a public method for wiring to a UI Skip button — clears all pending cards and jumps directly to Draw.
- Phase auto-advances to Draw once the pending list is empty (no extra click needed when all cards are resolved).

### Cards
- **Polyamorous Unicorn** — Magical Unicorn / `EVERY_TURN`. At the start of your turn, click the card to move it into the opponent's stable, then steal a unicorn from that stable.

### New Action Types
- **`MoveSelfToOpponentStableAction`** — synchronously moves `sourceCard` from the active player's unicorn stable to the opponent's unicorn stable. No player input required.
- **`StealUnicornAction`** — prompts the active player to click a card in the opponent's stable; moves the selected card to the active player's unicorn stable. Uses new `PendingActionType.StealCard`, handled in `Stable.HandleCardClick`.

### Fixes
- **Sync action chaining:** `CardActionExecutor.ExecuteNextAction()` now automatically chains to the next queued action when a synchronous action completes (i.e. `Execute()` returned without setting a `PendingActionType`). Previously, only the first sync action in a multi-step sequence would fire; subsequent actions were silently dropped.

### UI
- **`PhaseIndicator`** — new `MonoBehaviour` that drives a `TMP_Text` label with the current phase and active card name. Assign `TurnManager` and a `TMP_Text` in the Inspector.

### Docs
- `design-decisions.md`: EVERY_TURN gating section updated to reflect the player-driven activation model; new section documents the planned Option B full-screen overlay UI for `EveryTurnSpecial` (future work).

---

## [v0.2.5] — 2026-06-03

### Cards
- **Dumpster Diving Unicorn** — Magical Unicorn/IMMEDIATE. When played, draws the top card of the discard pile into the active player's hand (skipped if discard pile is empty). Uses the new `TakeFromDiscardAction`. `unicornType = SPECIAL`.

### New Action Types
- **`TakeFromDiscardAction`** — synchronously moves the top card of the discard pile to the active player's hand. Calls `RevealCard()` on the taken card. No player input required. Skips silently if the discard pile is empty.

### Fixes
- **Sync IMMEDIATE action stuck-turn bug:** `HandStable.HandleCardClick` now checks whether a pending action is still active after `PlayCard` returns. If the action chain completed synchronously (no pending action set), it passes `SpecialActionType.NONE` to `StartNextTurnPhase` instead of `IMMEDIATE`, preventing the game from parking in `ImmediateSpecial` with no way to advance. Previously affected Unicorn Enema and any future card with a fully-synchronous IMMEDIATE action.

### Tooling
- **`/add-card` skill** updated to include a `Force<CardName>ToTop()` debug stacking step in `DeckManager` as part of every new card implementation.
- **`ForceDumpsterDivingUnicornToTop()`** added to `DeckManager.Start()` so the card is always on top of the draw pile in Play mode.
- **M4 tracked** in `docs/issues.md`: Dumpster Diving Unicorn currently takes only the top card; a card-picker UI for choosing any card from the discard pile is deferred.

---

## [v0.2.4] — 2026-05-30

### Cards
- **Unicorn Enema** — Magic/IMMEDIATE. Sacrifices all Downgrade cards in the active player's stable. Uses `SacrificeCardAction { targetStable=Downgrade, sacrificeAll=true }`.
- **Breaking and Entering** — Magic/IMMEDIATE. Pulls 2 cards at random from the opponent's hand into the active player's hand. Uses `PullCardAction { numberOfCards=2 }`.

### New Action Types
- **`SacrificeCardAction`** — moves cards from the active player's own stables (Unicorn/Upgrade/Downgrade/Any) to the discard pile. `sacrificeAll=true` moves all cards automatically with no player prompt; `PlayerChooses` mode is deferred.
- **`PullCardAction`** — randomly moves N cards from the opponent's hand to the active player's hand. Fully automatic. Capped at opponent's hand size to handle edge cases gracefully.

### Deck Utility
- **`Deck.MoveToTop(Card)`** — single method that moves any card to the top of a deck, keeping `spaceCards` and the Transform sibling hierarchy in sync. Eliminates the previous two-step manual process (edit list in Inspector + move GameObject in scene). `ForceFmkToTop` and the new `ForceBreakingAndEnteringToTop` debug helpers both use it.

### Tooling
- **`/add-card` skill** — slash command that takes a card description and walks through the implementation decision tree: CardType → SpecialActionType → action mapping → C# class → Unity asset instructions.
- **`docs/cards/card-implementation-guide.md`** — full reference for implementing any card: decision tables, action-type parameter reference, class template, and worked examples.

---

## [v0.2.3] — 2026-04-29

### Fixes
- **M1:** Tracker correction — `ActivePlayerHasSpecialCards` and `ActivePlayerHasEveryTurnCards` were already removed in v0.2.0. `TurnManager.AdvanceToNextPlayerTurn` calls `CollectEveryTurnActions()` which correctly queries the three stables. No code change; tracker updated.
- **M2:** Added `public int winConditionCount = 7` to `UnicornStable`. `CheckWinCondition` now checks `spaceCards.Count >= winConditionCount` instead of `== maxCardsInStable`, decoupling the game rule from the UI layout capacity. Game-over UI not yet wired.
- **M3 (partial):** `HandStable.PositionCardsInStable` now clamps `displayCount = Mathf.Min(spaceCards.Count, 7)` before computing the fan layout, preventing the `ArgumentOutOfRangeException` that fired on 9+ cards. Cards beyond index 6 are unpositioned; a proper overflow formula or scroll is deferred.

---

## [v0.2.2] — 2026-04-25

### Bugfixes
- **B4:** `DestroyCardAction.TargetStable` enum and per-stable targeting removed. The destroying player now picks any card from any of the opponent's three stables (unicorn/upgrade/downgrade) instead of being locked to a single stable. `CardActionExecutor.ExecutePendingAction` falls back to `card.cardSpace` as the source when `pendingSourceStable` is null, so the actual stable is derived from the clicked card. FMK's "Kill" step (previously `targetStable = Unicorn`) now allows targeting upgrades and downgrades as well.

### Refactors
- **R1:** Destroy-pending-action click guard moved from each stable subclass into `Stable.HandleCardClick`. `UnicornStable`, `UpgradeStable`, and `DowngradeStable` previously held three near-identical copies (Unicorn's had drifted with two stray `Debug.Log` lines). The leaf overrides are deleted; `HandStable` retains its own override since hand-click semantics differ.
- **R2:** Introduced abstract `StackedStable : Stable` to host the edge-stacked overlap layout shared by `UpgradeStable` and `DowngradeStable`. Subclasses now declare a `protected override bool FromRightEdge` (true for upgrade, false for downgrade) and inherit a `sealed` `PositionCardsInStable`. The duplicated RectTransform setup, overlap math, and a stale `// 20% overlap` comment are gone; the `sealed` modifier prevents future copy-paste drift.
- **R3:** Removed the duplicated `cardType`, `specialActionType`, and `afterAction` fields from `Card` (and the matching copy lines in `Initialize`). `CardData` is now the single source of truth — `CardManager.PlayCardForCurrentPlayer`, `TurnManager.CollectEveryTurnActionsFromSpace`, and `HandStable.HandleCardClick` now read through `card.cardData.*` directly. Eliminates the risk of the cached enums drifting from the underlying `CardData` asset and drops `afterAction` from `Card` entirely (it was never read; see R5).
- **R4:** Collapsed `PromptPlayerToSelectAndDiscardCards`, `PromptPlayerToSelectAndGiveCards`, and `PromptPlayerToSelectAndDestroyCards` on `CardActionExecutor` into a single `PromptPlayerToSelectCards(player, source, destination, numberOfCards, actionType)`. The three old methods differed only in their log string, the `PendingActionType` they assigned, and (for Destroy) hardcoding `pendingSourceStable = null`. Callers in `DiscardCardAction`, `GiveCardAction`, and `DestroyCardAction` now pass the `PendingActionType` and source explicitly — `DestroyCardAction` passes `source = null` to defer source resolution to click time (the mechanism behind the B4 fix).
- **R5:** Removed the unused `AfterAction` enum and the `afterAction` field from `CardData`. The enum was set on every card in `OnEnable` (Magic/Neigh → `DISCARD`, Unicorn/Upgrade/Downgrade → `PLACE_IN_STABLE`) but never read — `CardManager.PlayCardForCurrentPlayer` routes purely on `cardType`. The dead state had also drifted: `DowngradeCardData` was marked `PLACE_IN_STABLE` despite routing to the opponent's stable, and `PLACE_IN_ENEMY_STABLE` was defined but never assigned. Deleted: `AfterAction.cs` (and `.meta`), the field, the five `OnEnable` assignments, the design-decisions section justifying the separation, and the matching CLAUDE.md "Known Gaps" row and enum reference.

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
