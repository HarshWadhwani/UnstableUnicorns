# Cult Leader Unicorn

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
> "When this card enters your Stable, each player (including you) must SACRIFICE a Unicorn card."

## Action Mapping
NEW: AllPlayersSacrificeAction { targetStable=Unicorn } — forces all players (including active player) to sacrifice a unicorn card.

## Passive Interfaces
None

## Implementation Notes
Targets ALL players including the active player — not just opponent. Requires a multi-player sacrifice action not currently implemented.
