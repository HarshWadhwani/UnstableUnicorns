# Buck Naked

| Field | Value |
|-------|-------|
| wiki_type | Magic |
| card_type | MAGIC |
| copies | 1 |
| trigger | IMMEDIATE |
| can_play | opponent has at least one Upgrade card in their Stable |
| impl_status | implemented (untested in Play mode) |
| impl_class | BuckNakedCardData.cs |

## Effect (2nd Edition)
> "DESTROY all Upgrade cards in another player's Stable."

## Action Mapping
- `DestroyCardAction { destroyer=ActivePlayer, targetStable=Upgrade, destroyAll=true }` — auto-moves every card in the opponent's Upgrade stable to the discard pile, no prompt.

## Passive Interfaces
None

## CanPlay Override
```csharp
opponentPlayer != null && opponentPlayer.upgradeStable.spaceCards.Count > 0
```

## Implementation Notes
Needed two additions to `DestroyCardAction`: an `Upgrade` value on the `TargetStable` enum, and a `destroyAll` bool that mirrors `SacrificeCardAction.sacrificeAll` — when set, every card in scope is moved to discard immediately with no player selection, and `numberOfCards` is ignored. Sacrifice shields (`ISacrificeShield`) are **not** consulted on a `destroyAll` — a mass destroy has no single target for a shield to intercept. Interactive (`destroyAll=false`) + `targetStable=Upgrade` is explicitly unsupported (logs a warning and no-ops); wiring it would need a new `PendingActionType` and `Stable.HandleCardClick` changes that no card currently requires.
