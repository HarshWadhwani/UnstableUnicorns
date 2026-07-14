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

**Current limit:** the hardcoded arrays only cover 1–7 cards. `PositionCardsInStable()` now caps `displayCount = Mathf.Min(spaceCards.Count, 7)` and only positions that many (see `docs/issues.md` M3) — it no longer throws, but cards beyond index 6 aren't visible or clickable. This is an intentional soft cap for the 2-player game (hand size rarely exceeds 7–8 in practice). If player count or hand limits change, this will need revisiting — either extend the arrays, switch to a formula for counts >7, or add scroll/overflow UI.

---

## `CardData.actions` are populated in `OnEnable()`, not in the Unity Inspector

**Files:** All `CardData` subclasses, especially `FuckMarryKillCardData.cs`

Card effects (the `List<CardAction> actions`) are defined in code inside `OnEnable()` rather than configured per-asset via the Inspector.

Defining them in code keeps effect logic version-controlled in git alongside the card class. If they were Inspector-configured, each `.asset` file would contain the serialized action list — divergence between asset instances of the same card type would be easy to introduce accidentally and hard to diff.

The tradeoff: adding a new card requires writing a new `CardData` subclass even if the effect could theoretically be assembled from existing `CardAction` primitives. This was a deliberate choice to keep each card's logic self-contained and auditable.

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
- **Parameter on `StartNextTurnPhase`** — `HandStable` passes `card.cardData.specialActionType` directly (the field lived on `Card` itself before the R3 cleanup removed the duplicate; see `docs/issues.md` R3). The information flows through the call stack without any intermediate state.

The parameter approach was chosen. The default value `SpecialActionType.NONE` means callers that don't care (e.g. the Deck advancing from Draw to Action) don't need to pass anything.

---

## `EveryTurnSpecial` fires before Draw, not after Action

**Files:** `TurnManager.cs`

EVERY_TURN effects in the physical card game trigger "at the start of your turn", which is before you draw. Placing `EveryTurnSpecial` after the Action phase of the previous player's turn (i.e. post-action, pre-switch) would misattribute the effect to the wrong player's turn and make the ordering confusing.

The chosen order is: switch player → check for EVERY_TURN actions → `EveryTurnSpecial` (if any) → `Draw`. This matches the physical game's turn structure and means the effect always fires in the context of the player whose turn it affects.

---

## `EveryTurnSpecial` entry is gated on collected cards, not on action count

**Files:** `TurnManager.cs`

`AdvanceToNextPlayerTurn()` collects the full list of EVERY_TURN cards from the new active player's stables before deciding whether to enter `EveryTurnSpecial`. The phase only activates if any cards with non-empty action lists are found.

An earlier version auto-executed actions immediately on phase entry. The current model is player-driven: cards sit in a pending list and the player activates each one by clicking it in their stable. Cards not clicked before the player presses Skip are skipped for that turn.

**Update:** this description now applies only to the `pendingChoiceCards` queue (Unicorn/Upgrade cards). Downgrade cards were split into a separate `pendingMandatoryCards` queue that auto-executes without a click and ignores Skip — see the entry below.

A card is removed from `pendingChoiceCards` the moment it is activated. When the executor finishes that card's actions (`StartNextTurnPhase(EveryTurnSpecial)` is called), it checks if any pending cards remain — if none, it advances to `Draw` automatically.

---

## Downgrade `EVERY_TURN` effects are mandatory; Unicorn/Upgrade `EVERY_TURN` effects are optional

**Files:** `TurnManager.cs`, `PhaseIndicator.cs`

The physical game's rule text distinguishes the two: Downgrade cards read "DISCARD a card" (an order), while Magical Unicorn/Upgrade `EVERY_TURN` cards typically read "you may..." (a choice). The original single `everyTurnCardsPending` list treated both the same way — the player had to click every EVERY_TURN card individually, and could press Skip to bypass a Downgrade's forced effect entirely, which is a rules violation.

**Fix:** `TurnManager` now collects Downgrade `EVERY_TURN` cards into `pendingMandatoryCards` and Unicorn/Upgrade `EVERY_TURN` cards into `pendingChoiceCards`, gathered separately in `AdvanceToNextPlayerTurn`. Whenever `pendingMandatoryCards` is non-empty, `ActivateNextMandatoryCard` fires the next one directly (no player click needed to *start* it — the underlying `CardAction`, e.g. `DiscardCardAction`, may still prompt the player to *choose which card*, that's unrelated to whether the effect itself is optional). `TryActivateEveryTurnCard` and `SkipEveryTurnPhase` both refuse to run while `pendingMandatoryCards` is non-empty, so a Unicorn/Upgrade choice card can't jump the queue and Skip can't be used to dodge a forced Downgrade. `TurnManager.CanSkipEveryTurnPhase` exposes this as a bool so `PhaseIndicator` can disable the Skip button in the UI rather than merely no-op the click.

**Silent skip when impossible:** no new code was needed for "skip if the player has 0 cards to discard" — `DiscardCardAction.Execute` already returns without setting a pending action when the target's hand is empty, and `CardActionExecutor.ExecuteNextAction` chains straight to the next action when nothing is pending. This applies to any current or future action used on a mandatory Downgrade, not just discard.

---

## Planned: EVERY_TURN overlay for optional effect activation (Option B)

**Files:** TBD — new overlay UI, `TurnManager.cs`

When `EveryTurnSpecial` is active, the intended UX is a dedicated overlay screen showing all the active player's EVERY_TURN cards spread out evenly. The player taps individual cards to activate their effects and taps a Skip/Done button when finished.

**Why Option B over a per-card prompt (Option A):**  
Option A (yes/no prompt before each card) generates one forced interaction per card per turn even when the player always wants to trigger every effect — it slows turns down with multiple EVERY_TURN cards. Option B gives the player all choices at once and requires only one interaction (Done/Skip) when they want everything or nothing.

**Current interim behaviour:** No overlay exists yet. The player activates an EVERY_TURN card by clicking it directly in their stable during `EveryTurnSpecial` phase. A Skip button on the main HUD ends the phase early (now disabled while any mandatory Downgrade card is pending — see the mandatory/optional split entry above). The `pendingChoiceCards` list in `TurnManager` (private; this note predates its rename from `everyTurnCardsPending` and its split from the mandatory Downgrade queue) is the source of truth for which optional cards are still available to activate.

**Prerequisites for the full overlay:**
- A new scene overlay Canvas that reads `pendingChoiceCards` (would need a public accessor) and renders card thumbnails
- Card thumbnails need click handlers that call `turnManager.TryActivateEveryTurnCard(card)`
- A "Done / Skip All" button on the overlay that calls `turnManager.SkipEveryTurnPhase()`
- The overlay activates/deactivates based on `turnManager.currentPhase == EveryTurnSpecial`

---

## Card data is loaded dynamically via `Resources.LoadAll`, not a manually wired Inspector list

**Files:** `DeckManager.cs`

`DeckManager` previously had a serialized `List<CardData> playCardDatas` field that required manually adding each new card asset in the Unity Inspector. This meant adding a new card was a two-step process: create the asset, then remember to wire it up.

`Resources.LoadAll<CardData>("CardDataInstances")` at runtime discovers all card assets automatically. The only requirement is that the asset lives in the correct `Resources/` subdirectory. Each `CardData` asset already has an `instances` field that controls how many copies appear in the deck, so the loader respects that without any additional configuration.
