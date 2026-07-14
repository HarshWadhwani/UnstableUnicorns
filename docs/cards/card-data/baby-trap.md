# Baby Trap

| Field | Value |
|-------|-------|
| wiki_type | Magic |
| card_type | MAGIC |
| copies | 2 |
| trigger | IMMEDIATE |
| can_play | opponent has a Baby Unicorn in their Stable |
| impl_status | done |
| impl_class | BabyTrapCardData.cs |

## Effect (2nd Edition)
> "STEAL a Baby Unicorn card."

## Action Mapping
- StealUnicornAction {} — restricted to Baby Unicorn subtype

## Passive Interfaces
None

## CanPlay Override
```csharp
opponentPlayer.unicornStable.Cards.Any(c => c.CardData is UnicornCardData u && u.unicornType == UnicornType.Baby)
```

## Implementation Notes
`StealUnicornAction` gained an optional `targetSubtype` field (`UnicornType?`, default `null` = any). Baby Trap sets `targetSubtype = UnicornType.BABY`. `CardActionExecutor.pendingStealSubtypeFilter` carries the filter through the click-prompt step; `Stable.HandleCardClick`'s `StealCard` branch now also requires `this is UnicornStable` (previously any of the opponent's three stables could be clicked during a steal prompt — a latent bug fixed alongside this card) and rejects a click that doesn't match the pending subtype.

`CanPlay` requires the opponent's unicorn stable to contain at least one `UnicornType.BABY` card.
