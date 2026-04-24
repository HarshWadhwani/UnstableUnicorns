# Issues & Tech Debt

Tracked issues from code and design review. Work through these one by one — mark **Status** when done.

---

## Bugs

### B1 — `DeckManager.ShuffleDeck` doesn't actually shuffle draw order
**Status:** Open  
**File:** `DeckManager.cs`  
The shuffle reorders Unity Transform siblings but `CardSpace.spaceCards` is a separate `List<Card>` that is never reordered. Since `Deck.HandleCardClick` draws `spaceCards[0]`, the shuffle has zero effect on gameplay.  
**Fix:** After reordering siblings, rebuild `spaceCards` from the new sibling order, or shuffle `spaceCards` directly using Fisher-Yates.

---

### B2 — `Stable.PositionCardsInStable` has an off-by-one in card placement
**Status:** Open  
**File:** `Stable.cs`  
`leftMostOpenPosition` is incremented before placing a card only when `i > 0 && spaceCards.Count > 1`. For a 2-card stable this skips placing the first card at its correct slot center — both cards end up shifted right by one slot width.  
**Fix:** Compute position as `startX + i * cardSlotWidth` unconditionally, remove the conditional increment.

---

### B3 — `TriggerSpecialAction` overrides on leaf classes suppress the base implementation
**Status:** Open  
**Files:** `BabyUnicornCardData.cs`, `BasicNeighCardData.cs`, `DiscardNeighCardData.cs`, `FinalNeighCardData.cs`, `UpgradeCardData.cs`, `DowngradeCardData.cs`  
Each of these overrides `TriggerSpecialAction` with only a `Debug.Log`, discarding the base class logic that executes the `actions` list. Any actions added to these card types in the future will silently do nothing.  
**Fix:** Remove these overrides entirely (the base class handles it), or add `base.TriggerSpecialAction(sourceCard)` calls.

---

### B4 — `DestroyCardAction` with `TargetStable.Any` silently falls back to Unicorn stable
**Status:** Open  
**File:** `DestroyCardAction.cs`  
`TargetStable.Any` is unimplemented and defaults to `unicornStable` with a warning log. Any card using `Any` will incorrectly target only unicorns.  
**Fix:** Implement `Any` — likely requires the destroying player to choose which stable to target, which needs a new `PendingActionType`.

---

## Redundancies / Cleanup

### R1 — `UpgradeStable.HandleCardClick` and `DowngradeStable.HandleCardClick` are identical
**Status:** Open  
**Files:** `UpgradeStable.cs`, `DowngradeStable.cs`  
Both have the exact same 8-line destroy-pending-action guard. Any future change must be made in two places.  
**Fix:** Move this logic into `Stable` as a virtual or shared protected method.

---

### R2 — `UpgradeStable.PositionCardsInStable` and `DowngradeStable.PositionCardsInStable` are near-identical
**Status:** Open  
**Files:** `UpgradeStable.cs`, `DowngradeStable.cs`  
The only differences are the overlap direction sign and the starting X anchor. All RectTransform setup is duplicated.  
**Fix:** Extract into a shared `Stable` method with a `bool stackFromRight` (or `int direction`) parameter.

---

### R3 — `Card` duplicates fields already on `CardData`
**Status:** Open  
**File:** `Card.cs`  
`Card` stores `cardType`, `specialActionType`, and `afterAction` locally and copies them from `cardData` in `Initialize()`. This creates two sources of truth for the same value.  
**Fix:** Remove the duplicate fields from `Card`; read from `card.cardData.*` everywhere. Update any references.

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
