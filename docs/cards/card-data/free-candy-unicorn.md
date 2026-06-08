# Free Candy Unicorn

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
> "When this card enters your Stable, STEAL a Baby Unicorn card. If this card leaves your Stable, return that Baby Unicorn card to the Stable from which you stole it."

## Action Mapping
- StealUnicornAction {} — but restricted to Baby Unicorn subtype

## Passive Interfaces
None

## Implementation Notes
Steal is specifically a Baby Unicorn (not any Unicorn). Also has a "leave stable" triggered return effect — requires tracking which Baby Unicorn was stolen and where it came from. Complex state tracking not currently supported.
