# Fuck Marry Kill

**Type:** Magic  
**Script:** `Assets/Scripts/CardData/MagicCardData/FuckMarryKillCardData.cs`  
**Asset:** `Assets/Resources/CardDataInstances/MagicCardDataInstances/Fuck Marry Kill Card Data.asset`

---

## Physical Card Effect

> Choose a unicorn in any player's Stable to DESTROY, choose a player to SACRIFICE a card, and choose a player to GIVE a card from their hand.

The digital implementation reads the effect as a fixed sequence targeting the opponent:
1. Opponent discards 1 card from their hand (chosen by the opponent)
2. Active player gives 1 card from their hand to the opponent (chosen by the active player)
3. Active player destroys 1 unicorn from the opponent's unicorn stable (chosen by the active player)

---

## Implementation

`FuckMarryKillCardData` extends `MagicCardData`, which sets:
- `cardType = MAGIC`
- `specialActionType = IMMEDIATE`
- `afterAction = DISCARD`

`OnEnable()` populates `actions` with three `CardAction` instances in sequence:

```
actions[0] = DiscardCardAction  { targetPlayer = Opponent, selectionMode = PlayerChooses, numberOfCards = 1 }
actions[1] = GiveCardAction     { giver = ActivePlayer, numberOfCards = 1 }
actions[2] = DestroyCardAction  { destroyer = ActivePlayer, targetStable = Unicorn, numberOfCards = 1 }
```

### Execution flow

1. Player plays the card from `HandStable` → `CardManager.PlayCardForCurrentPlayer()` is called
2. Because `specialActionType == IMMEDIATE`, `CardManager` calls `card.cardData.TriggerSpecialAction(card)` before routing the card to the discard pile
3. `TriggerSpecialAction` → `CardActionExecutor.ExecuteActions(actions, sourceCard)`
4. Executor creates a `CardActionContext` (captures activePlayer, opponentPlayer, sourceCard, managers), enqueues all 3 actions, calls `ExecuteNextAction()`

**Step 1 — DiscardCardAction (opponent chooses):**
- `DiscardCardAction.Execute()` identifies `opponentPlayer.handStable` as the source
- `selectionMode == PlayerChooses` → calls `executor.PromptPlayerToSelectAndDiscardCards(opponentPlayer, handStable, discardPile, 1)`
- Executor saves `originalActivePlayer`, reassigns `turnManager.activePlayer = opponentPlayer`
- Sets `currentPendingAction = DiscardCard`, `pendingCardsRemaining = 1`
- Queue is paused — waiting for the opponent to click a card

- Opponent clicks a card in their hand → `HandStable.HandleCardClick()` detects `currentPendingAction == DiscardCard`
- Calls `executor.ExecutePendingAction(card)` → `CardManager.MoveCard(card, handStable, discardPile)`
- `pendingCardsRemaining` hits 0 → `ClearPendingAction()` restores `originalActivePlayer`, calls `ExecuteNextAction()`

**Step 2 — GiveCardAction (active player chooses what to give):**
- `GiveCardAction.Execute()` identifies `activePlayer.handStable` as source, `opponentPlayer.handStable` as destination
- Calls `executor.PromptPlayerToSelectAndGiveCards(activePlayer, handStable, opponentHandStable, 1)`
- `turnManager.activePlayer` is reassigned to `activePlayer` (same player, no change in this case)
- Active player clicks a card in their hand → same `HandStable.HandleCardClick()` path with `GiveCard` pending type
- `MoveCard` moves the card to the opponent's hand

**Step 3 — DestroyCardAction (active player chooses which unicorn to destroy):**
- `DestroyCardAction.Execute()` with `destroyer = ActivePlayer`, `targetStable = Unicorn`
- Determines target player = opponent (destroyer is active, so target is opponent)
- Gets `opponentPlayer.unicornStable` as the target space
- Calls `executor.PromptPlayerToSelectAndDestroyCards(activePlayer, unicornStable, discardPile, 1)`
- Active player clicks a card in the opponent's `UnicornStable` → `UnicornStable.HandleCardClick()` detects `DestroyCard` pending
- Guards: if `card.cardSpace.player == turnManager.activePlayer` → warns and returns (can't destroy own unicorns)
- Otherwise: `executor.ExecutePendingAction(card)` → moves unicorn to discardPile

After all 3 steps, `CardActionExecutor` queue is empty. `HandStable` then calls `turnManager.StartNextTurnPhase()` to advance the turn (this call is made in `HandStable.HandleCardClick` after `PlayCard()` returns successfully — before any pending action completes, but only once per card play).

---

## Quirks & Edge Cases

**The card routes to discard pile before effects resolve.** `CardManager.PlayCardForCurrentPlayer()` calls `TriggerSpecialAction` first, then moves the played card to the discard pile. This means the card is physically moved to the discard pile mid-effect, which is correct for Magic cards.

**GiveCardAction destination is always opponent's hand.** The physical card says "choose a player to give a card" — in a 2-player game this always means the opponent, which is how `GiveCardAction` currently resolves it (giver's hand → opponent's hand). This is correct for 2 players but will need a player-selection step for 3+ players.

**Turn phase advances before effects finish.** `HandStable.HandleCardClick` calls `turnManager.StartNextTurnPhase()` after `PlayCard()` returns `true`, which happens before the `CardActionExecutor` queue runs to completion. The turn phase advances immediately, but the card effect continues running as an overlay on top of the new phase. This works in practice because no CardSpace checks the turn phase during a pending action — but it's a latent ordering issue.

**DestroyCardAction.TargetStable.Any is not implemented.** If a future card uses `DestroyCardAction` with `targetStable = Any`, it defaults to `unicornStable` with a warning. FMK hardcodes `targetStable = Unicorn` so this doesn't affect it.

---

## Testing Checklist

- [ ] Opponent's hand shrinks by 1 after Step 1
- [ ] Active player's hand shrinks by 1 after Step 2; opponent's hand grows by 1
- [ ] Opponent's unicorn stable shrinks by 1 after Step 3; discardPile grows by 1
- [ ] Cannot select own cards during destroy step (warning fires correctly)
- [ ] Turn advances to next phase after effect resolves
- [ ] Playing FMK when opponent has 0 cards in hand — Step 1 should log and skip gracefully
