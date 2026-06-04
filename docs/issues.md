# Issues & Tech Debt

Tracked issues from code and design review. Work through these one by one.

**For Claude:** When the user declares an issue complete, update its **Status** line and the tracker table below in the same response.

---

## Tracker

| ID | Description | Status |
|----|-------------|--------|
| B1 | `ShuffleDeck` doesn't shuffle draw order | ✅ Fixed |
| B2 | `Stable.PositionCardsInStable` off-by-one | ✅ Fixed |
| B3 | `TriggerSpecialAction` overrides suppress base | ✅ Fixed |
| B4 | `DestroyCardAction.Any` silently falls back to Unicorn | ✅ Fixed |
| R1 | `UpgradeStable`/`DowngradeStable` identical `HandleCardClick` | ✅ Fixed |
| R2 | `UpgradeStable`/`DowngradeStable` near-identical `PositionCardsInStable` | ✅ Fixed |
| R3 | `Card` duplicates fields from `CardData` | ✅ Fixed |
| R4 | Three near-identical `PromptPlayer*` methods | ✅ Fixed |
| R5 | `AfterAction` enum never read | ✅ Fixed |
| M1 | Special phase checks hand instead of stables | ✅ Fixed |
| M2 | Win condition tied to `maxCardsInStable` | ✅ Fixed |
| M3 | Hand size >8 throws exception | ⚠️ Partial |
| M4 | Dumpster Diving Unicorn draws top of discard only; should let player pick any card | Open |

---

## Bugs

### B1 — `DeckManager.ShuffleDeck` doesn't actually shuffle draw order
**Status:** Fixed  
**File:** `DeckManager.cs`  
The shuffle reorders Unity Transform siblings but `CardSpace.spaceCards` is a separate `List<Card>` that is never reordered. Since `Deck.HandleCardClick` draws `spaceCards[0]`, the shuffle has zero effect on gameplay.  
**Fix:** After reordering siblings, rebuild `spaceCards` from the new sibling order, or shuffle `spaceCards` directly using Fisher-Yates.

---

### B2 — `Stable.PositionCardsInStable` has an off-by-one in card placement
**Status:** Fixed  
**File:** `Stable.cs`  
`leftMostOpenPosition` is incremented before placing a card only when `i > 0 && spaceCards.Count > 1`. For a 2-card stable this skips placing the first card at its correct slot center — both cards end up shifted right by one slot width.  
**Fix:** Compute position as `startX + i * cardSlotWidth` unconditionally, remove the conditional increment.

---

### B3 — `TriggerSpecialAction` overrides on leaf classes suppress the base implementation
**Status:** Fixed  
**Files:** `BabyUnicornCardData.cs`, `BasicNeighCardData.cs`, `DiscardNeighCardData.cs`, `FinalNeighCardData.cs`, `UpgradeCardData.cs`, `DowngradeCardData.cs`  
Each of these overrides `TriggerSpecialAction` with only a `Debug.Log`, discarding the base class logic that executes the `actions` list. Any actions added to these card types in the future will silently do nothing.  
**Fix:** Remove these overrides entirely (the base class handles it), or add `base.TriggerSpecialAction(sourceCard)` calls.

---

### B4 — `DestroyCardAction` with `TargetStable.Any` silently falls back to Unicorn stable
**Status:** Fixed  
**File:** `DestroyCardAction.cs`  
`TargetStable.Any` was unimplemented and defaulted to `unicornStable` with a warning log. Any card using `Any` would have incorrectly targeted only unicorns.  
**Fix applied:** Removed the `TargetStable` enum entirely — every `DestroyCardAction` now lets the destroyer pick any card from any of the opposing player's three stables. `CardActionExecutor.ExecutePendingAction` falls back to `card.cardSpace` when `pendingSourceStable` is null, so the source is resolved at click time. FMK's `targetStable = Unicorn` line was removed (its "Kill" step now allows targeting upgrades and downgrades).

---

## Redundancies / Cleanup

### R1 — `UpgradeStable.HandleCardClick` and `DowngradeStable.HandleCardClick` are identical
**Status:** Fixed  
**Files:** `Stable.cs`, `UpgradeStable.cs`, `DowngradeStable.cs`, `UnicornStable.cs`  
All three stable subclasses had the same destroy-pending-action guard (Unicorn's copy had drifted with two stray `Debug.Log` lines). Any future change to destroy-click semantics would have needed three edits.  
**Fix applied:** Moved the destroy guard into `Stable.HandleCardClick` (previously a no-op). Removed the override from `UpgradeStable`, `DowngradeStable`, and `UnicornStable`. `HandStable` keeps its own override since hand-click semantics differ.

---

### R2 — `UpgradeStable.PositionCardsInStable` and `DowngradeStable.PositionCardsInStable` are near-identical
**Status:** Fixed  
**Files:** `StackedStable.cs` (new), `UpgradeStable.cs`, `DowngradeStable.cs`  
The two methods differed only in the anchor edge and overlap direction sign; all RectTransform setup was duplicated. A stale `// 20% overlap` comment in `UpgradeStable` had also drifted from the actual 50% overlap math.  
**Fix applied:** Introduced an intermediate abstract `StackedStable : Stable` that owns the layout, exposing a `protected abstract bool FromRightEdge` for subclasses to declare their direction. `PositionCardsInStable` is `sealed override` on `StackedStable` to prevent the same drift recurring. `UpgradeStable` and `DowngradeStable` collapse to one-line declarations of their direction.

---

### R3 — `Card` duplicates fields already on `CardData`
**Status:** Fixed  
**Files:** `Card.cs`, `CardManager.cs`, `TurnManager.cs`, `HandStable.cs`  
`Card` stored `cardType`, `specialActionType`, and `afterAction` locally and copied them from `cardData` in `Initialize()`, creating two sources of truth.  
**Fix applied:** Removed the three fields from `Card` and the matching copies from `Initialize()`. The four call sites (`CardManager.PlayCardForCurrentPlayer` for both `cardType` and `specialActionType`, `TurnManager.CollectEveryTurnActionsFromSpace`, and `HandStable.HandleCardClick`) now read through `card.cardData.*`. `afterAction` is no longer cached on `Card` (it was never read anywhere — see R5).

---

### R4 — `CardActionExecutor` has three near-identical `PromptPlayer*` methods
**Status:** Fixed  
**Files:** `CardActionExecutor.cs`, `DiscardCardAction.cs`, `GiveCardAction.cs`, `DestroyCardAction.cs`  
`PromptPlayerToSelectAndDiscardCards`, `PromptPlayerToSelectAndGiveCards`, and `PromptPlayerToSelectAndDestroyCards` were identical except for the log string, the `PendingActionType` assigned, and (Destroy) hardcoding `pendingSourceStable = null`.  
**Fix applied:** Collapsed into one method `PromptPlayerToSelectCards(Player, CardSpace source, CardSpace destination, int numberOfCards, PendingActionType actionType)`. Callers pass the `PendingActionType` and the source explicitly. `DestroyCardAction` passes `source = null` to defer source resolution to click time (the `??` fallback to `card.cardSpace` in `ExecutePendingAction` — the mechanism behind B4).

---

### R5 — `AfterAction` enum is defined but never read
**Status:** Fixed  
**Files:** `AfterAction.cs` (deleted), `CardData.cs`, `MagicCardData.cs`, `NeighCardData.cs`, `UnicornCardData.cs`, `UpgradeCardData.cs`, `DowngradeCardData.cs`, `CLAUDE.md`, `docs/design-decisions.md`, `docs/cards/fuck-marry-kill.md`  
`AfterAction` values were set on every card but `CardManager.PlayCardForCurrentPlayer` routes purely on `cardType` — the enum was never consulted. The dead state had also drifted: `DowngradeCardData` was marked `PLACE_IN_STABLE` despite routing to the opponent's stable, and `PLACE_IN_ENEMY_STABLE` was defined but never assigned to any card.  
**Fix applied:** Deleted `AfterAction.cs` and `.meta`, the `afterAction` field on `CardData`, and the five `OnEnable` assignments. Removed the design-decisions section justifying the (now nonexistent) separation, the CLAUDE.md "Known Gaps" entry for `PLACE_IN_ENEMY_STABLE`, and stale enum/`afterAction` references in CLAUDE.md and `fuck-marry-kill.md`. `cardType` is now the sole driver of post-play routing.

---

## Missing Mechanics

### M1 — `TurnManager.ActivePlayerHasSpecialCards` checks hand instead of stables
**Status:** Fixed (tracker was stale — already resolved in code)  
**File:** `TurnManager.cs`  
Both `ActivePlayerHasSpecialCards` and `ActivePlayerHasEveryTurnCards` were removed during the v0.2.0 refactor. `AdvanceToNextPlayerTurn` now calls `CollectEveryTurnActions()`, which correctly queries `unicornStable`, `upgradeStable`, and `downgradeStable` — the right places for EVERY_TURN effects.

---

### M2 — Win condition tied to `maxCardsInStable` (visual capacity)
**Status:** Fixed  
**File:** `UnicornStable.cs`  
`CheckWinCondition` was winning when `spaceCards.Count == maxCardsInStable`, coupling the game rule to the UI layout capacity. Also used `==` instead of `>=`, so if a card were ever added beyond max the win would never fire.  
**Fix applied:** Added `public int winConditionCount = 7` to `UnicornStable`. `CheckWinCondition` now checks `spaceCards.Count >= winConditionCount`. `maxCardsInStable` remains the layout/cap value. Game-over UI not yet wired — `CheckWinCondition` still logs only.

---

### M4 — Dumpster Diving Unicorn draws top of discard only
**Status:** Open  
**File:** `TakeFromDiscardAction.cs`  
The card text says "you may add a card from the discard pile to your hand", implying free choice. Currently `TakeFromDiscardAction` always takes the top card automatically — no player selection, no visibility into the pile.  
**Fix direction:** Reveal discard pile contents (scrollable list or card-picker UI), prompt the active player to click a card, then use `PromptPlayerToSelectCards` with a new `PendingActionType.TakeFromDiscard` wired through `DiscardPile.HandleCardClick`.

---

### M3 — Hand size >8 throws `ArgumentOutOfRangeException`
**Status:** Partial  
**File:** `HandStable.cs`  
`GetYPositionOffsetValue` only handles 1–8 cards and throws for anything beyond. There was no guard in `PositionCardsInStable`.  
**Partial fix applied:** `PositionCardsInStable` now caps `displayCount = Mathf.Min(spaceCards.Count, 7)` and loops only over those cards. Cards beyond index 6 are not positioned (they remain off-screen or wherever Unity left them). The crash is prevented, but overflow cards are not visible or accessible. A parabolic formula fallback (or proper scroll/overflow UI) is deferred.
