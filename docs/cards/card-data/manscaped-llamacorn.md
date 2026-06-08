# Manscaped Llamacorn

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
> "When this card enters your Stable, each other player must DISCARD a card."

## Action Mapping
- DiscardCardAction { targetPlayer=Opponent, selectionMode=PlayerChooses, numberOfCards=1 }

## Passive Interfaces
None

## Implementation Notes
"Each other player" — in 2-player this is just the opponent. Multi-player would require iterating all opponents.
