# Putting on a Show

| Field | Value |
|-------|-------|
| wiki_type | Upgrade |
| card_type | UPGRADE |
| copies | 2 |
| trigger | EVERY_TURN |
| can_play | always |
| impl_status | not_started |
| impl_class | — |

## Effect (2nd Edition)
> "If this card is in your Stable at the beginning of your turn, you may SACRIFICE a card, then DESTROY a card."

## Action Mapping
- SacrificeCardAction { targetStable=Any, sacrificeAll=false }
- DestroyCardAction { destroyer=ActivePlayer, targetStable=Any, numberOfCards=1 }

## Passive Interfaces
None

## Implementation Notes
Optional. Sacrifice any card from own stable as cost, then destroy any card from opponent's stable. Similar to Sadomasocorn but destroy is not restricted to Unicorns.
