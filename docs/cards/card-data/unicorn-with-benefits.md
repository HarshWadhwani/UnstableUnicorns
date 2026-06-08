# Unicorn with Benefits

| Field | Value |
|-------|-------|
| wiki_type | Upgrade |
| card_type | UPGRADE |
| copies | 1 |
| trigger | EVERY_TURN |
| can_play | requires a Basic Unicorn card in your Stable |
| impl_status | not_started |
| impl_class | — |

## Effect (2nd Edition)
> "You can only play this card if there is a Basic Unicorn card in your Stable. If this card is in your Stable at the beginning of your turn, you may bring a Basic Unicorn card from your hand into your Stable."

## Action Mapping
NEW: PlayCardFromHandToStableAction { cardType=UNICORN, unicornSubtype=Basic } — active player may play a Basic Unicorn from hand directly into their stable.

## Passive Interfaces
None

## CanPlay Override
```csharp
activePlayer.unicornStable.Cards.Any(c => c.CardData is UnicornCardData u && u.unicornType == UnicornType.Basic)
```

## Implementation Notes
Optional. Brings a Basic Unicorn from hand to stable without using the Action phase. CanPlay requires a Basic Unicorn already in stable.
