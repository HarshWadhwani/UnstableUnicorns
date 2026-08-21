# Putting on a Show

| Field | Value |
|-------|-------|
| wiki_type | Upgrade |
| card_type | UPGRADE |
| copies | 2 |
| trigger | EVERY_TURN |
| can_play | always |
| impl_status | done |
| impl_class | PuttingOnAShowCardData.cs |

## Effect (2nd Edition)
> "If this card is in your Stable at the beginning of your turn, you may SACRIFICE a card, then DESTROY a card."

## Action Mapping
- SacrificeCardAction { targetStable=Any, sacrificeAll=false }
- DestroyCardAction { destroyer=ActivePlayer, targetStable=Any, numberOfCards=1 }

## Passive Interfaces
None

## CanActivateEveryTurn Override
```csharp
opponentPlayer.unicornStable.spaceCards.Count > 0
    || opponentPlayer.upgradeStable.spaceCards.Count > 0
    || opponentPlayer.downgradeStable.spaceCards.Count > 0
```
Only guards the destroy side — the sacrifice side (`targetStable=Any`) always has an eligible target since this card, being an Upgrade sitting in the active player's own Upgrade stable, is itself a valid self-sacrifice candidate whenever the EVERY_TURN check runs.

## Implementation Notes
Optional. Sacrifice any card from own stable as cost, then destroy any card from opponent's stable. Similar to Sadomasocorn but destroy is not restricted to Unicorns.
