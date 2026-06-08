# Twinkicorn

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
> "When this card enters your Stable, you may search the deck for a 'Bear Daddy Unicorn' card and add it to your hand, then shuffle the deck."

## Action Mapping
NEW: SearchDeckForNamedCardAction { cardName="Bear Daddy Unicorn", destination=Hand } — searches deck for a specific named card and adds it to hand, then shuffles.

## Passive Interfaces
None

## Implementation Notes
Pairs with Bear Daddy Unicorn (which has the reverse effect). Requires a deck search by card name.
