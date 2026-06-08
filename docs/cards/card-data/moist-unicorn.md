# Moist Unicorn

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
> "When this card enters your Stable, you may search the deck for a Unicorn card and add it to your hand, then shuffle the deck."

## Action Mapping
NEW: SearchDeckForTypeAction { cardType=UNICORN, destination=Hand } — player searches deck for any Unicorn card and adds it to hand, then shuffles.

## Passive Interfaces
None

## Implementation Notes
Deck search by card type — not currently supported by any action type.
