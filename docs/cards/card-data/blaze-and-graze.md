# Blaze and Graze

| Field | Value |
|-------|-------|
| wiki_type | Magic |
| card_type | MAGIC |
| copies | 1 |
| trigger | IMMEDIATE |
| can_play | always |
| impl_status | done |
| impl_class | BlazeAndGrazeCardData.cs |

## Effect (2nd Edition)
> "Reveal the top card in the deck. If it is a Unicorn card, bring it into your Stable. If it is anything else, add it to your hand."

## Action Mapping
RevealTopDeckAction {} — reveals the top card of the play deck; if `CardType.UNICORN`, moves it to the active player's Unicorn stable (and checks win condition); otherwise moves it to the active player's hand. No parameters — the routing is fixed by the card's own effect text. Skips silently if the deck is empty.

## Passive Interfaces
None

## Implementation Notes
No player prompt — fully automatic, same style as `TakeFromDiscardAction`.
