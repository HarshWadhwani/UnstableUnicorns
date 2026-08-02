# Bukkakecorn

| Field | Value |
|-------|-------|
| wiki_type | Magical Unicorn |
| card_type | UNICORN |
| unicorn_subtype | MAGIC |
| copies | 1 |
| trigger | EVERY_TURN |
| can_play | always |
| impl_status | done |
| impl_class | BukkakecornCardData.cs |

## Effect (2nd Edition)
> "If this card is in your Stable at the beginning of your turn, you may DISCARD 3 cards, then STEAL a Unicorn card."

## Action Mapping
- DiscardCardAction { targetPlayer=ActivePlayer, selectionMode=PlayerChooses, numberOfCards=3 }
- StealUnicornAction {}

## Passive Interfaces
None

## CanActivateEveryTurn Override
```csharp
opponentPlayer.unicornStable.spaceCards.Count >= 1 && activePlayer.handStable.spaceCards.Count >= 3
```
Prevents activating with fewer than 3 cards in hand (which would otherwise leave the discard prompt stuck waiting for a card that doesn't exist — `DiscardCardAction` doesn't cap `numberOfCards` to hand size) or with no opponent unicorn to steal. Checked in `TurnManager.TryActivateEveryTurnCard` before the action queue runs at all — an all-or-nothing gate, not a per-action skip, so it can't partially discard then fail to steal.

## Implementation Notes
Optional effect — player may choose not to discard. Cost is 3 discards; StealUnicornAction already exists.
