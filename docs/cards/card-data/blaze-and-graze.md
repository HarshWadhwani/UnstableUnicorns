# Blaze and Graze

| Field | Value |
|-------|-------|
| wiki_type | Magic |
| card_type | MAGIC |
| copies | 1 |
| trigger | IMMEDIATE |
| can_play | always |
| impl_status | not_started |
| impl_class | — |

## Effect (2nd Edition)
> "Reveal the top card in the deck. If it is a Unicorn card, bring it into your Stable. If it is anything else, add it to your hand."

## Action Mapping
NEW: RevealTopDeckAction { ifUnicorn=BringToStable, otherwise=AddToHand } — reveal top card; conditional routing based on card type.

## Passive Interfaces
None

## Implementation Notes
Conditional effect based on revealed card type. Requires branching logic not supported by existing action types.
