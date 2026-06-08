# Hoof Job

| Field | Value |
|-------|-------|
| wiki_type | Magic |
| card_type | MAGIC |
| copies | 2 |
| trigger | IMMEDIATE |
| can_play | opponent has at least one card in hand |
| impl_status | not_started |
| impl_class | — |

## Effect (2nd Edition)
> "Look at another player's hand. Choose a card and add it to your hand."

## Action Mapping
NEW: LookAndTakeFromHandAction { targetPlayer=Opponent, cardTypeFilter=Any } — active player views opponent's full hand and chooses any card to take.

## Passive Interfaces
None

## CanPlay Override
```csharp
opponentPlayer.handStable.Cards.Count > 0
```

## Implementation Notes
Similar to Entitled Unicorn but takes any card type (not just Unicorn). Requires viewing the hand and selecting any card.
