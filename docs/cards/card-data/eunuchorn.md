# Eunuchorn

| Field | Value |
|-------|-------|
| wiki_type | Magical Unicorn |
| card_type | UNICORN |
| unicorn_subtype | MAGIC |
| copies | 1 |
| trigger | NONE |
| can_play | always |
| impl_status | not_started |
| impl_class | — |

## Effect (2nd Edition)
> "Baby Unicorn cards cannot enter any player's Stable."

## Action Mapping
NEW: passive ability — while this card is in your stable, no Baby Unicorn cards can enter any player's stable.

## Passive Interfaces
None

## Implementation Notes
Global passive effect. Requires a game rule override/hook when Baby Unicorns would enter any stable. No existing interface handles this.
