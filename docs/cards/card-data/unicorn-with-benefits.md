# Unicorn with Benefits

| Field | Value |
|-------|-------|
| wiki_type | Upgrade |
| card_type | UPGRADE |
| copies | 1 |
| trigger | EVERY_TURN |
| can_play | requires a Basic Unicorn card in your Stable |
| impl_status | done |
| impl_class | UnicornWithBenefitsCardData.cs |

## Effect (2nd Edition)
> "You can only play this card if there is a Basic Unicorn card in your Stable. If this card is in your Stable at the beginning of your turn, you may bring a Basic Unicorn card from your hand into your Stable."

## Action Mapping
- PlayCardFromHandAction { cardType=UNICORN, targetSubtype=BASIC }

## Passive Interfaces
None

## CanPlay Override
```csharp
activePlayer.unicornStable.spaceCards.Any(c => c.cardData is UnicornCardData u && u.unicornType == UnicornType.BASIC)
```

## Implementation Notes
Optional. Brings a Basic Unicorn from hand to stable without using the Action phase. CanPlay requires a Basic Unicorn already in stable.
