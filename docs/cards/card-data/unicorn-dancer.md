# Unicorn Dancer

| Field | Value |
|-------|-------|
| wiki_type | Magical Unicorn |
| card_type | UNICORN |
| unicorn_subtype | MAGIC |
| copies | 1 |
| trigger | EVERY_TURN |
| can_play | always |
| impl_status | done |
| impl_class | UnicornDancerCardData.cs |

## Effect (2nd Edition)
> "If this card is in your Stable at the beginning of your turn, you may DRAW a card and DISCARD a card."

## Action Mapping
- DrawCardAction { numberOfCards=1 }
- DiscardCardAction { targetPlayer=ActivePlayer, selectionMode=PlayerChooses, numberOfCards=1 }

## Passive Interfaces
None

## CanActivateEveryTurn Override
```csharp
DeckManager.Instance.playDeck.spaceCards.Count > 0
```
Without this, an empty play deck would let the draw silently fail while the discard still fires — losing a card for nothing.

## Implementation Notes
Draw 1 then discard 1 — net neutral card exchange. `DrawCardAction` draws from the top of the active player's own play deck (mirrors `CardManager.DrawCard`, no player prompt).
