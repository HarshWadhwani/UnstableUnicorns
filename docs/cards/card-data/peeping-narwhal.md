# Peeping Narwhal

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
> "When this card enters your Stable, each other player must show you their hand."

## Action Mapping
NEW: RevealHandsAction {} — forces all other players to reveal their hands to the active player.

## Passive Interfaces
None

## Implementation Notes
Information-only effect; no cards move. Requires a UI reveal mechanic. In 2-player, just reveals the opponent's hand.
