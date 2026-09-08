# Fuzzy Hoofcuffs

| Field | Value |
|-------|-------|
| wiki_type | Upgrade |
| card_type | UPGRADE |
| copies | 1 |
| trigger | EVERY_TURN |
| can_play | always |
| impl_status | done |
| impl_class | FuzzyHoofcuffsCardData.cs |

## Effect (2nd Edition)
> "If this card is in your Stable at the beginning of your turn, you may discard 2 cards, then steal a Unicorn card."

## Action Mapping
- DiscardCardAction { targetPlayer=ActivePlayer, selectionMode=PlayerChooses, numberOfCards=2 }
- StealUnicornAction {}

## Passive Interfaces
None

## CanActivateEveryTurn Override
```csharp
opponentPlayer.unicornStable.spaceCards.Count >= 1 && activePlayer.handStable.spaceCards.Count >= 2
```
Fixed 2026-09-07 — originally shipped without this guard, which meant activating with a hand of 0-1 cards left the discard prompt permanently stuck (and unskippable, since Skip disables while a prompt is pending). Same pattern as Bukkakecorn.

## Implementation Notes
Optional. Cost is 2 discards from own hand. StealUnicornAction already exists. Functionally similar to Bukkakecorn (3 discards) but costs 1 less.
