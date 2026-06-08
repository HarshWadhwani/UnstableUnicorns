# Buck Naked

| Field | Value |
|-------|-------|
| wiki_type | Magic |
| card_type | MAGIC |
| copies | 1 |
| trigger | IMMEDIATE |
| can_play | opponent has at least one Upgrade card in their Stable |
| impl_status | not_started |
| impl_class | — |

## Effect (2nd Edition)
> "DESTROY all Upgrade cards in another player's Stable."

## Action Mapping
- DestroyCardAction { destroyer=ActivePlayer, targetStable=Any, numberOfCards=999 } — destroy all upgrades (would need targetStable=Upgrade and sacrificeAll=true equivalent)

## Passive Interfaces
None

## CanPlay Override
```csharp
opponentPlayer.upgradeStable.Cards.Count > 0
```

## Implementation Notes
Destroys ALL Upgrade cards (not just one). DestroyCardAction currently targets a specific stable card interactively — would need an auto-destroy-all variant for Upgrade type.
