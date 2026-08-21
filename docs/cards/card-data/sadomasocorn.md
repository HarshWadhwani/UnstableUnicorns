# Sadomasocorn

| Field | Value |
|-------|-------|
| wiki_type | Magical Unicorn |
| card_type | UNICORN |
| unicorn_subtype | MAGIC |
| copies | 1 |
| trigger | EVERY_TURN |
| can_play | always |
| impl_status | done |
| impl_class | SadomasocornCardData.cs |

## Effect (2nd Edition)
> "If this card is in your Stable at the beginning of your turn, you may SACRIFICE a card, then DESTROY a Unicorn card."

## Action Mapping
- SacrificeCardAction { targetStable=Any, sacrificeAll=false }
- DestroyCardAction { destroyer=ActivePlayer, targetStable=Unicorn, numberOfCards=1 }

## Passive Interfaces
None

## CanActivateEveryTurn Override
```csharp
opponentPlayer.unicornStable.spaceCards.Count > 0
```
Only guards the destroy side — the sacrifice side (`targetStable=Any`) always has an eligible target since this card, being a Unicorn sitting in the active player's own Unicorn stable, is itself a valid self-sacrifice candidate whenever the EVERY_TURN check runs.

## Implementation Notes
Optional — player may choose not to trigger. The sacrifice cost can be any card (targetStable=Any). Must destroy a Unicorn card from opponent's stable.
