# Unicorn Flatulence

| Field | Value |
|-------|-------|
| wiki_type | Downgrade |
| card_type | DOWNGRADE |
| copies | 1 |
| trigger | EVERY_TURN |
| can_play | always |
| impl_status | not_started |
| impl_class | — |

## Effect (2nd Edition)
> "If this card is in your Stable at the beginning of your turn, return a Unicorn card from your Stable to your hand. If at any time you have no Unicorn cards in your Stable, SACRIFICE this card."

## Action Mapping
NEW: ReturnToHandFromOwnStableAction { targetStable=Unicorn } — active player returns one Unicorn from their own stable to their hand (mandatory).
NEW: AutoSacrificeIfNoUnicronsAction {} — passive check: if stable has no unicorns, auto-sacrifice this card.

## Passive Interfaces
None

## Implementation Notes
Mandatory EVERY_TURN effect. Also has a conditional self-sacrifice trigger when the stable reaches 0 unicorns. The auto-sacrifice should fire immediately whenever that condition is true, not just at turn start.
