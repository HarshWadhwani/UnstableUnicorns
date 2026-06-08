# Blow Up Unicorn

| Field | Value |
|-------|-------|
| wiki_type | Upgrade |
| card_type | UPGRADE |
| copies | 2 |
| trigger | NONE |
| can_play | always |
| impl_status | not_started |
| impl_class | — |

## Effect (2nd Edition)
> "If a Unicorn card in your Stable would be sacrificed or destroyed, you may SACRIFICE this card instead."

## Action Mapping
NEW: passive ability — intercepts a sacrifice or destroy targeting any unicorn in your stable; you may sacrifice this card to prevent it.

## Passive Interfaces
ISacrificeShield (card auto-sacrifices to intercept a destroy)

## Implementation Notes
Similar to Hentaicorn but also intercepts sacrifices (not just destroys). ISacrificeShield currently only covers destroys — may need to be extended to cover sacrifice events too.
