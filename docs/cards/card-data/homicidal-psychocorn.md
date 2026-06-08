# Homicidal Psychocorn

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
> "If this card is in your Stable at the beginning of your turn, you may DISCARD your hand, then DESTROY a Unicorn card."

## Action Mapping
- DiscardCardAction { targetPlayer=ActivePlayer, selectionMode=Random, numberOfCards=999 } — discard entire hand (all cards)
- DestroyCardAction { destroyer=ActivePlayer, targetStable=Unicorn, numberOfCards=1 }

## Passive Interfaces
None

## CanPlay Override
None (always playable as a unicorn card itself)

## Implementation Notes
Optional effect. "DISCARD your hand" means discard ALL cards in hand — numberOfCards should be set to hand size or a high cap. The discard is mandatory if the effect is chosen (not optional per card). Requires opponent to have at least one Unicorn card for the destroy to be meaningful (but effect can still be triggered).
