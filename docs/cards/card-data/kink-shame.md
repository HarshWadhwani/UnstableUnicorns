# Kink Shame

| Field | Value |
|-------|-------|
| wiki_type | Magic |
| card_type | MAGIC |
| copies | 3 |
| trigger | IMMEDIATE |
| can_play | opponent has an Upgrade card OR active player has a Downgrade card |
| impl_status | not_started |
| impl_class | — |

## Effect (2nd Edition)
> "DESTROY an Upgrade card or SACRIFICE a Downgrade card."

## Action Mapping
NEW: PlayerChoosesEffectAction — player picks one of two options: DestroyCardAction { destroyer=ActivePlayer, targetStable=Any (Upgrade only), numberOfCards=1 } OR SacrificeCardAction { targetStable=Downgrade, sacrificeAll=false }.

## Passive Interfaces
None

## CanPlay Override
```csharp
opponentPlayer.upgradeStable.Cards.Count > 0 || activePlayer.downgradeStable.Cards.Count > 0
```

## Implementation Notes
Player chooses one of two effects at play time. "DESTROY an Upgrade" targets opponent's upgrade stable. "SACRIFICE a Downgrade" targets active player's own downgrade stable. Requires a branching player-choice action not currently implemented.
