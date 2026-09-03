# Unicorn Speed

| Field | Value |
|-------|-------|
| wiki_type | Upgrade |
| card_type | UPGRADE |
| copies | 2 |
| trigger | EVERY_TURN |
| can_play | requires a Basic Unicorn card in your Stable |
| impl_status | done |
| impl_class | UnicornSpeedCardData.cs |

## Effect (2nd Edition)
> "You can only play this card if there is a Basic Unicorn card in your Stable. If this card is in your Stable at the beginning of your turn, you may DRAW a card."

## Action Mapping
- DrawCardAction { numberOfCards=1 }

## Passive Interfaces
None

## CanPlay Override
```csharp
activePlayer.unicornStable.spaceCards.Any(c => c.cardData is UnicornCardData u && u.unicornType == UnicornType.BASIC)
```

## Implementation Notes
EVERY_TURN draw. `CanPlay` restriction requires at least one Basic Unicorn in stable. Single-action effect (no partial-effect risk), so no `CanActivateEveryTurn` override needed.
