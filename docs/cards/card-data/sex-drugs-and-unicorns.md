# Sex, Drugs, and Unicorns

| Field | Value |
|-------|-------|
| wiki_type | Magic |
| card_type | MAGIC |
| copies | 1 |
| trigger | IMMEDIATE |
| can_play | opponent has at least 3 cards in hand OR at least 1 Unicorn in Stable |
| impl_status | not_started |
| impl_class | — |

## Effect (2nd Edition)
> "Choose a player. That player must either DISCARD 3 cards or SACRIFICE a Unicorn card."

## Action Mapping
NEW: OpponentChoosesEffectAction — targeted player chooses between: DiscardCardAction { targetPlayer=Opponent, selectionMode=PlayerChooses, numberOfCards=3 } OR SacrificeCardAction { targetStable=Unicorn, sacrificeAll=false }.

## Passive Interfaces
None

## CanPlay Override
```csharp
opponentPlayer.handStable.Cards.Count >= 3 || opponentPlayer.unicornStable.Cards.Count > 0
```

## Implementation Notes
The OPPONENT chooses which punishment to take (not the active player). Requires a new mechanic where the targeted player is presented with a choice.
