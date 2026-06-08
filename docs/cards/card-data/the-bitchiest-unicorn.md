# The Bitchiest Unicorn

| Field | Value |
|-------|-------|
| wiki_type | Magical Unicorn |
| card_type | UNICORN |
| unicorn_subtype | MAGIC |
| copies | 1 |
| trigger | EVERY_TURN |
| can_play | requires a Basic Unicorn card in your Stable |
| impl_status | not_started |
| impl_class | — |

## Effect (2nd Edition)
> "You can only play this card if there is a Basic Unicorn card in your Stable. If this card is in your Stable at the beginning of your turn, you may force another player to DISCARD a card."

## Action Mapping
- DiscardCardAction { targetPlayer=Opponent, selectionMode=PlayerChooses, numberOfCards=1 }

## Passive Interfaces
None

## CanPlay Override
```csharp
activePlayer.unicornStable.Cards.Any(c => c.CardData.unicornType == UnicornType.Basic)
```

## Implementation Notes
Optional EVERY_TURN effect. Has a CanPlay restriction requiring at least one Basic Unicorn in your stable.
