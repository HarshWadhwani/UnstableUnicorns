# Horrifying Impaling

| Field | Value |
|-------|-------|
| wiki_type | Magic |
| card_type | MAGIC |
| copies | 2 |
| trigger | IMMEDIATE |
| can_play | opponent has at least 1 Unicorn card in their Stable |
| impl_status | done |
| impl_class | HorrifyingImpalingCardData.cs |

## Effect (2nd Edition)
> "DESTROY a Unicorn card. Each player (including you) must DISCARD a card."

## Action Mapping
- DestroyCardAction { destroyer=ActivePlayer, targetStable=Unicorn, numberOfCards=1 }
- DiscardCardAction { targetPlayer=ActivePlayer, selectionMode=PlayerChooses, numberOfCards=1 }
- DiscardCardAction { targetPlayer=Opponent, selectionMode=PlayerChooses, numberOfCards=1 }

## Passive Interfaces
None

## CanPlay Override
```csharp
opponentPlayer.unicornStable.Cards.Count > 0
```
