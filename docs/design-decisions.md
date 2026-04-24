# Design Decisions

Full reasoning behind structural choices in the codebase. Reference when working on a specific subsystem.

---

## CardActionExecutor temporarily reassigns `turnManager.activePlayer` during pending actions

**Files:** `CardActionExecutor.cs`, `HandStable.cs`, `UnicornStable.cs`, `UpgradeStable.cs`, `DowngradeStable.cs`

When a card effect requires a player to choose a card (e.g., opponent discards from hand), `CardActionExecutor` saves the original `activePlayer` into `originalActivePlayer`, then reassigns `turnManager.activePlayer` to the target player.

This works because every `CardSpace.HandleCardClick` already guards against the wrong player clicking by checking `card.cardSpace.player == turnManager.activePlayer`. By temporarily making the target player the "active" player, all existing click-gating logic routes input to the right player's cards automatically — no separate input-mode flag needed.

The original player is restored in `ClearPendingAction()` once `pendingCardsRemaining` hits zero.

**Watch out for:** Any new `CardSpace` subclass that adds player-gating logic needs to be consistent with this pattern, or it will silently reject clicks during pending actions.

---

## `DowngradeStable` belongs to the victim's `Player`, not the attacker's

**Files:** `CardManager.cs`, `DowngradeStable.cs`, `Player.cs`

`CardManager.PlayCardForCurrentPlayer()` routes downgrade cards to `opponent.downgradeStable`, not `activePlayer.downgradeStable`. Downgrades sit in the stable of the player being penalized.

This keeps ownership semantics consistent: a player's `Player` object is the authoritative source for everything in their board state (hand, unicorns, upgrades, downgrades). If downgrades were stored on the attacker, every system that needs to evaluate a player's board (win conditions, EVERY_TURN effects, card counts) would need to check both players.

---

## `UpgradeStable` and `DowngradeStable` use opposite-side overlapping layouts

**Files:** `UpgradeStable.cs`, `DowngradeStable.cs`

Upgrades stack right-aligned; downgrades stack left-aligned. Both use 50% overlap so multiple cards remain partially visible.

The mirrored alignment is a visual design choice: on a player's board both rows are simultaneously visible, and the direction of the stack signals at a glance which type of effect it is. There's no functional difference — the layout math is the same, just anchored to opposite edges.

---

## `HandStable.PositionCardsInStable()` uses hardcoded Y-offset arrays

**Files:** `HandStable.cs` (line ~110–130)

Y positions for each card in the fan layout are stored as hardcoded arrays per card count (1–8 cards), rather than calculated from a formula.

A pure arc formula (y = r × sin(θ)) produces visually unnatural fans at low card counts (1–3 cards) where the arc is too subtle to read clearly. The hardcoded values were tuned manually per count.

**Current limit:** `PositionCardsInStable()` throws `ArgumentOutOfRangeException` for >8 cards. This is an intentional soft cap for the 2-player game (hand size rarely exceeds 7–8 in practice). If player count or hand limits change, this will need revisiting — either extend the arrays or switch to a formula for counts >8.

---

## `CardData.actions` are populated in `OnEnable()`, not in the Unity Inspector

**Files:** All `CardData` subclasses, especially `FuckMarryKillCardData.cs`

Card effects (the `List<CardAction> actions`) are defined in code inside `OnEnable()` rather than configured per-asset via the Inspector.

Defining them in code keeps effect logic version-controlled in git alongside the card class. If they were Inspector-configured, each `.asset` file would contain the serialized action list — divergence between asset instances of the same card type would be easy to introduce accidentally and hard to diff.

The tradeoff: adding a new card requires writing a new `CardData` subclass even if the effect could theoretically be assembled from existing `CardAction` primitives. This was a deliberate choice to keep each card's logic self-contained and auditable.

---

## `AfterAction` on `CardData` is separate from `CardAction` routing

**Files:** `CardData.cs`, `CardManager.cs`, all `CardAction` subclasses

`AfterAction` (DISCARD / PLACE_IN_STABLE / PLACE_IN_ENEMY_STABLE) describes where the **played card itself** goes after being played. `CardAction` instances describe the **effect the card has on other cards**.

These are intentionally separate concerns. A Magic card discards itself (`afterAction = DISCARD`) while also causing the opponent to discard a card from their hand (`DiscardCardAction`). Conflating the two would make it impossible to express "this card goes to your stable AND causes an opponent effect."

`AfterAction` is currently only partially used — `PLACE_IN_ENEMY_STABLE` is defined but never routed in `CardManager`. This would be the hook for cards that physically place themselves onto an opponent's board.

---

## `Special` is split into `ImmediateSpecial` and `EveryTurnSpecial`

**Files:** `TurnPhase.cs`, `TurnManager.cs`, `CardActionExecutor.cs`

The original single `Special` phase was used for both IMMEDIATE card effects (e.g. FMK) and was incorrectly being triggered by EVERY_TURN cards played from hand. These are fundamentally different concepts with different triggers and timing, so they became two separate phases.

`ImmediateSpecial` fires mid-turn when the active player plays an IMMEDIATE card. The turn pauses here while the executor runs to completion, then advances to the next player.

`EveryTurnSpecial` fires at the start of each player's turn before Draw, driven by what is already in their stables — not by what they just played. It is skipped entirely if there are no EVERY_TURN actions to run.

Keeping them separate means the `StartNextTurnPhase` switch can route each case independently with no flags or special-casing inside the executor.

**Watch out for:** Unity serializes enum values as integers. New `TurnPhase` values must always be added at the end of the enum to avoid shifting existing integer indices and breaking `allowedTurnPhases` lists serialized in the scene.

---

## `StartNextTurnPhase` receives `SpecialActionType` as a parameter rather than reading from a field

**Files:** `TurnManager.cs`, `HandStable.cs`

When deciding whether to enter `ImmediateSpecial` after the Action phase, `TurnManager` needs to know the `SpecialActionType` of the card that was just played. Two approaches were considered:

- **`lastPlayedCard` field on `TurnManager`** — `CardManager` sets it after a card is played; `TurnManager` reads it when advancing. This adds a field that exists solely to bridge one method call, creating a second source of truth for information that is already available at the call site.
- **Parameter on `StartNextTurnPhase`** — `HandStable` passes `card.specialActionType` directly. The information flows through the call stack without any intermediate state.

The parameter approach was chosen. The default value `SpecialActionType.NONE` means callers that don't care (e.g. the Deck advancing from Draw to Action) don't need to pass anything.

---

## `EveryTurnSpecial` fires before Draw, not after Action

**Files:** `TurnManager.cs`

EVERY_TURN effects in the physical card game trigger "at the start of your turn", which is before you draw. Placing `EveryTurnSpecial` after the Action phase of the previous player's turn (i.e. post-action, pre-switch) would misattribute the effect to the wrong player's turn and make the ordering confusing.

The chosen order is: switch player → check for EVERY_TURN actions → `EveryTurnSpecial` (if any) → `Draw`. This matches the physical game's turn structure and means the effect always fires in the context of the player whose turn it affects.

---

## `EveryTurnSpecial` entry is gated on collected actions, not on card count

**Files:** `TurnManager.cs`

`AdvanceToNextPlayerTurn()` collects the full list of EVERY_TURN actions from the new active player's stables before deciding whether to enter `EveryTurnSpecial`. The phase only activates if `everyTurnActions.Count > 0`.

An earlier version checked `ActivePlayerHasEveryTurnCards()` (card existence) first, then collected actions separately in `TriggerEveryTurnActions()`. This created a gap: a card with `specialActionType = EVERY_TURN` but an empty `actions` list would set the phase to `EveryTurnSpecial` but never start the executor — leaving the turn stuck permanently.

Collecting actions first and using the count as the gate eliminates the gap in one pass.

---

## Card data is loaded dynamically via `Resources.LoadAll`, not a manually wired Inspector list

**Files:** `DeckManager.cs`

`DeckManager` previously had a serialized `List<CardData> playCardDatas` field that required manually adding each new card asset in the Unity Inspector. This meant adding a new card was a two-step process: create the asset, then remember to wire it up.

`Resources.LoadAll<CardData>("CardDataInstances")` at runtime discovers all card assets automatically. The only requirement is that the asset lives in the correct `Resources/` subdirectory. Each `CardData` asset already has an `instances` field that controls how many copies appear in the deck, so the loader respects that without any additional configuration.
