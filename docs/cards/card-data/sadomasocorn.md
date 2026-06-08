# Sadomasocorn

| Field | Value |
|-------|-------|
| wiki_type | Magical Unicorn |
| card_type | UNICORN |
| unicorn_subtype | MAGIC |
| copies | 1 |
| trigger | EVERY_TURN |
| can_play | always |
| impl_status | not_started |
| impl_class | — |

## Effect (2nd Edition)
> "If this card is in your Stable at the beginning of your turn, you may SACRIFICE a card, then DESTROY a Unicorn card."

## Action Mapping
- SacrificeCardAction { targetStable=Any, sacrificeAll=false }
- DestroyCardAction { destroyer=ActivePlayer, targetStable=Unicorn, numberOfCards=1 }

## Passive Interfaces
None

## Implementation Notes
Optional — player may choose not to trigger. The sacrifice cost can be any card (targetStable=Any). Must destroy a Unicorn card from opponent's stable.
