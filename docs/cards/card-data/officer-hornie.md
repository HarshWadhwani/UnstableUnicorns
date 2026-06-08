# Officer Hornie

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
> "When this card enters your Stable, you may look at another player's hand. Choose a Unicorn card and move it to the discard pile."

## Action Mapping
NEW: LookAndDestroyFromHandAction { targetPlayer=Opponent, cardTypeFilter=Unicorn } — active player views opponent's hand and sends one Unicorn card to the discard pile.

## Passive Interfaces
None

## Implementation Notes
Similar to Entitled Unicorn but the chosen card goes to discard pile instead of active player's hand. Requires viewing the hand and selecting a specific Unicorn card.
