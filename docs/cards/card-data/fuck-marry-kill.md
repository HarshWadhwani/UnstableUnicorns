# Fuck. Marry. Kill

| Field | Value |
|-------|-------|
| wiki_type | Magic |
| card_type | MAGIC |
| copies | 2 |
| trigger | IMMEDIATE |
| can_play | opponent has at least 1 card in hand AND at least 1 card in their Stable |
| impl_status | done |
| impl_class | FuckMarryKillCardData.cs |

## Effect (2nd Edition)
> "Force another player to DISCARD a card, give another player a card from your hand, then DESTROY a Unicorn card."

## Action Mapping
- DiscardCardAction { targetPlayer=Opponent, selectionMode=PlayerChooses, numberOfCards=1 }
- GiveCardAction { giver=ActivePlayer, numberOfCards=1 }
- DestroyCardAction { destroyer=ActivePlayer, targetStable=Any, numberOfCards=1 }

## Passive Interfaces
None

## CanPlay Override
```csharp
opponentPlayer.handStable.Cards.Count > 0 && (opponentPlayer.unicornStable.Cards.Count > 0 || opponentPlayer.upgradeStable.Cards.Count > 0 || opponentPlayer.downgradeStable.Cards.Count > 0)
```
