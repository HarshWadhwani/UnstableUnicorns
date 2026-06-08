# Sticky Situation

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
> "If this card is in your Stable at the beginning of your turn, skip either your Draw phase or your Action Phase."

## Action Mapping
NEW: PlayerChoosesSkipPhaseAction { options=[Draw, Action] } — the affected player must choose to skip either their Draw phase or their Action Phase for this turn.

## Passive Interfaces
None

## Implementation Notes
Mandatory (not optional). The affected player must skip one of two phases each turn. Requires a phase-skip system in TurnManager. TurnManager.skipNextDrawPhase already exists — would need an equivalent for Action phase.
