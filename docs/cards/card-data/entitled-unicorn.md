# Entitled Unicorn

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
> "When this card enters your Stable, you may look at another player's hand. Choose a Unicorn card and add it to your hand."

## Action Mapping
NEW: LookAndTakeFromHandAction { targetPlayer=Opponent, cardTypeFilter=Unicorn } — active player views opponent's hand and takes one Unicorn card.

## Passive Interfaces
None

## Implementation Notes
Different from PullCardAction (which is random). This requires viewing the hand and choosing a specific Unicorn card type.
