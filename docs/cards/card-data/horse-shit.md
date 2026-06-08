# Horse Shit

| Field | Value |
|-------|-------|
| wiki_type | Downgrade |
| card_type | DOWNGRADE |
| copies | 1 |
| trigger | NONE |
| can_play | always |
| impl_status | not_started |
| impl_class | — |

## Effect (2nd Edition)
> "Unicorns in your Stable are considered Shits. Cards that affect Unicorn cards do not affect your Shits."

## Action Mapping
NEW: passive ability — all Unicorn cards in the affected player's stable are renamed "Shits" and are immune to effects that target Unicorn cards.

## Passive Interfaces
None

## Implementation Notes
Passive type-rename effect. Unicorns become a different "type" (Shits) for targeting purposes. Cards like DestroyCardAction { targetStable=Unicorn } would not be able to target these. Requires a type-override system on CardData or stable cards.
