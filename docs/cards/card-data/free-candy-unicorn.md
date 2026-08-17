# Free Candy Unicorn

| Field | Value |
|-------|-------|
| wiki_type | Magical Unicorn |
| card_type | UNICORN |
| unicorn_subtype | MAGIC |
| copies | 1 |
| trigger | IMMEDIATE |
| can_play | always |
| impl_status | done |
| impl_class | FreeCandyUnicornCardData.cs |

## Effect (2nd Edition)
> "When this card enters your Stable, STEAL a Baby Unicorn card. If this card leaves your Stable, return that Baby Unicorn card to the Stable from which you stole it."

## Action Mapping
- StealUnicornAction { targetSubtype=BABY }

## Passive Interfaces
- `IReturnsStolenCardOnLeave` — marker only, no methods. When the steal resolves, `CardActionExecutor.ExecutePendingAction` records the stolen card + its origin stable onto `Card.linkedBabyUnicorn`/`linkedBabyUnicornOriginStable` (only ever for a Baby Unicorn — checked via `UnicornType.BABY`, so this can't fire for any other card's steal). `CardManager.MoveCard` checks for the interface whenever a card leaves a `UnicornStable`; if a link is set and the linked Baby Unicorn is still sitting where it was left, it's moved back to its origin. If the Baby Unicorn was itself discarded/destroyed/moved elsewhere in the meantime, the return is skipped silently rather than resurrecting it.

## Implementation Notes
Steal is specifically a Baby Unicorn (not any Unicorn) — implemented via `StealUnicornAction.targetSubtype`, same pattern as Baby Trap. The "leave stable" return effect is now fully implemented (see Passive Interfaces above). Because `TriggerSpecialAction` for a `UNICORN`-type `IMMEDIATE` card fires *before* `CardManager.PlayCardForCurrentPlayer` moves the card from hand into the Unicorn stable, the steal resolves while Free Candy Unicorn is still in hand — so the entry move never false-triggers the leave-hook (`oldCardSpace` is `handStable`, not a `UnicornStable`).
