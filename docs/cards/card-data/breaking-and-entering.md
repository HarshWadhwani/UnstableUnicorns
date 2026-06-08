# Breaking and Entering

| Field | Value |
|-------|-------|
| wiki_type | Magic |
| card_type | MAGIC |
| copies | 2 |
| trigger | IMMEDIATE |
| can_play | opponent has at least 1 card in hand |
| impl_status | done |
| impl_class | BreakingAndEnteringCardData.cs |

## Effect (2nd Edition)
> "Pull 2 cards at random from another player's hand."

## Action Mapping
- PullCardAction { numberOfCards=2, skipDrawPhaseOnSuccess=false }

## Passive Interfaces
None

## CanPlay Override
```csharp
opponentPlayer.handStable.Cards.Count > 0
```
