# Sextra-Terrestrial Unicorn

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
> "When this card enters your Stable, you may return a card in another player's Stable to their hand."

## Action Mapping
NEW: ReturnToHandFromStableAction { targetPlayer=Opponent, targetStable=Any } — active player chooses a card from opponent's stable and returns it to that opponent's hand.

## Passive Interfaces
None

## Implementation Notes
Optional. Returns any card (unicorn, upgrade, or downgrade) from opponent's stable to opponent's own hand — not to active player's hand.
