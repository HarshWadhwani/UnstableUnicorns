# Unicorn Cuckold

| Field | Value |
|-------|-------|
| wiki_type | Magical Unicorn |
| card_type | UNICORN |
| unicorn_subtype | MAGIC |
| copies | 1 |
| trigger | EVERY_TURN |
| can_play | always |
| impl_status | not_started |
| impl_class | — |

## Effect (2nd Edition)
> "If this card is in your Stable at the beginning of your turn, move a Unicorn card from your Stable into any other player's Stable. At the end of your turn, return that Unicorn card to your Stable."

## Action Mapping
NEW: LoanUnicornAction { from=ActivePlayer, to=Opponent } — temporarily moves a unicorn from active player's stable to opponent's stable, then returns it at end of turn.

## Passive Interfaces
None

## Implementation Notes
Requires end-of-turn tracking to return the loaned unicorn. The loaned card temporarily counts toward the opponent's stable during their turn. Complex state management needed.
