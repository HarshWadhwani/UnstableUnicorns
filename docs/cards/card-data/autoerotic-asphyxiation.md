# Autoerotic Asphyxiation

| Field | Value |
|-------|-------|
| wiki_type | Downgrade |
| card_type | DOWNGRADE |
| copies | 1 |
| trigger | EVERY_TURN |
| can_play | always |
| impl_status | done |
| impl_class | AutoeroticAsphyxiationCardData.cs |

## Effect (2nd Edition)
> "If this card is in your Stable at the beginning of your turn, DISCARD a card."

## Action Mapping
- DiscardCardAction { targetPlayer=ActivePlayer, selectionMode=PlayerChooses, numberOfCards=1 }

## Passive Interfaces
None

## Implementation Notes
Mandatory (not optional) — active player must discard 1 card each turn. Applied to the player who has this Downgrade in their stable.

Implemented via `TurnManager`'s new mandatory/choice split for `EVERY_TURN` cards: all Downgrade `EVERY_TURN` cards auto-fire in `AdvanceToNextPlayerTurn` without waiting for a stable click, and the Skip button is disabled/no-ops while any are pending (see `docs/design-decisions.md`). `DiscardCardAction` already returns silently when the target's hand is empty, so the "as long as it's possible" clause needed no new code.
