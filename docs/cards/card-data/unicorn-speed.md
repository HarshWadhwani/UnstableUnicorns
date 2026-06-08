# Unicorn Speed

| Field | Value |
|-------|-------|
| wiki_type | Upgrade |
| card_type | UPGRADE |
| copies | 2 |
| trigger | EVERY_TURN |
| can_play | requires a Basic Unicorn card in your Stable |
| impl_status | not_started |
| impl_class | — |

## Effect (2nd Edition)
> "You can only play this card if there is a Basic Unicorn card in your Stable. If this card is in your Stable at the beginning of your turn, you may DRAW a card."

## Action Mapping
NEW: DrawCardAction { numberOfCards=1 }

## Passive Interfaces
None

## CanPlay Override
```csharp
activePlayer.unicornStable.Cards.Any(c => c.CardData is UnicornCardData u && u.unicornType == UnicornType.Basic)
```

## Implementation Notes
Optional EVERY_TURN draw. CanPlay restriction requires at least one Basic Unicorn in stable. DrawCardAction not yet implemented as a CardAction type.
