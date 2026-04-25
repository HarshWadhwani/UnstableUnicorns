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
| R4 | Three near-identical `PromptPlayer*` methods | 🔲 Open |
| R5 | `AfterAction` enum never read | 🔲 Open |
| M1 | Special phase checks hand instead of stables | 🔲 Open |
| M2 | Win condition tied to `maxCardsInStable` | 🔲 Open |
| M3 | Hand size >8 throws exception | 🔲 Open |

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
**Status:** Open  
**File:** `CardActionExecutor.cs`  
`PromptPlayerToSelectAndDiscardCards`, `PromptPlayerToSelectAndGiveCards`, and `PromptPlayerToSelectAndDestroyCards` are identical except for the `PendingActionType` assigned.  
**Fix:** Collapse into one method: `PromptPlayerToSelectCards(Player, CardSpace source, CardSpace dest, int count, PendingActionType)`.

---

### R5 — `AfterAction` enum is defined but never read
**Status:** Open  
**Files:** `CardData.cs`, `CardManager.cs`  
`AfterAction` values are set on every card but `CardManager.PlayCardForCurrentPlayer` uses a `CardType` switch to route cards — `afterAction` is never consulted. The enum is redundant with the switch.  
**Fix:** Either route using `afterAction` in `CardManager` (replacing the switch), or remove the enum and keep the switch. Pick one source of truth.

---

## Missing Mechanics

### M1 — `TurnManager.ActivePlayerHasSpecialCards` checks hand instead of stables
**Status:** Open  
**File:** `TurnManager.cs`  
The method checks hand cards for non-`NONE` `SpecialActionType`, but the Special phase is for EVERY_TURN effects on cards **already in play** (upgrade/downgrade stables). `ActivePlayerHasEveryTurnCards()` exists and checks the right places but is never called.  
**Fix:** Replace the `ActivePlayerHasSpecialCards` call in `StartNextTurnPhase` with `ActivePlayerHasEveryTurnCards`.

---

### M2 — Win condition tied to `maxCardsInStable` (visual capacity)
**Status:** Open  
**File:** `UnicornStable.cs`  
`CheckWinCondition` wins when `spaceCards.Count == maxCardsInStable`. The win threshold is 7 unicorns in the actual game rules, which should be a separate constant independent of how many cards fit visually in the UI.  
**Fix:** Add a `winConditionCount` field (default 7) to `UnicornStable` and check against that instead.

---

### M3 — Hand size >8 throws `ArgumentOutOfRangeException`
**Status:** Open  
**File:** `HandStable.cs`  
`GetYPositionOffsetValue` only handles 1–8 cards and throws for anything beyond. There's no guard in `PositionCardsInStable`.  
**Fix:** Add a fallback for counts >8 (either a formula or clamping to 8 visible + scroll).
