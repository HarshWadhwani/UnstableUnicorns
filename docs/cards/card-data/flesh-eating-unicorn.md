# Flesh-Eating Unicorn

| Field | Value |
|-------|-------|
| wiki_type | Magical Unicorn |
| card_type | UNICORN |
| unicorn_subtype | MAGIC |
| copies | 1 |
| trigger | IMMEDIATE |
| can_play | always |
| impl_status | not_started |
| impl_class | — |

## Effect (2nd Edition)
> "When this card enters your Stable, you may choose any player. That player must DISCARD 2 cards."

## Action Mapping
- DiscardCardAction { targetPlayer=Opponent, selectionMode=PlayerChooses, numberOfCards=2 }

## Passive Interfaces
None

## Implementation Notes
"Choose any player" — in 2-player this is always the opponent. Multi-player would need a player-selection step.
