# Baby Trap

| Field | Value |
|-------|-------|
| wiki_type | Magic |
| card_type | MAGIC |
| copies | 2 |
| trigger | IMMEDIATE |
| can_play | opponent has a Baby Unicorn in their Stable |
| impl_status | not_started |
| impl_class | — |

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
StealUnicornAction currently steals any unicorn. Would need a targetSubtype=Baby restriction. CanPlay guard needed since the card is pointless if opponent has no Baby Unicorns.
