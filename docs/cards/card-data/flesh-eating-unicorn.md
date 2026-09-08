# Flesh-Eating Unicorn

| Field | Value |
|-------|-------|
| wiki_type | Magical Unicorn |
| card_type | UNICORN |
| unicorn_subtype | MAGIC |
| copies | 1 |
| trigger | IMMEDIATE |
| can_play | always |
| impl_status | done |
| impl_class | FleshEatingUnicornCardData.cs |

## Effect (2nd Edition)
> "When this card enters your Stable, you may choose any player. That player must DISCARD 2 cards."

## Action Mapping
- DiscardCardAction { targetPlayer=Opponent, selectionMode=PlayerChooses, numberOfCards=2 }

## Passive Interfaces
None

## CanPlay Override
```csharp
opponentPlayer.handStable.spaceCards.Count >= 2
```
Fixed 2026-09-07 — originally shipped without this guard, which meant playing it against an opponent with 0-1 cards in hand left the discard prompt permanently stuck. Unlike an EVERY_TURN choice card, this is IMMEDIATE, so `CanPlay` (not `CanActivateEveryTurn`) is the only available gate — the active player has no in-game signal stopping them from playing it regardless of opponent hand size.

## Implementation Notes
"Choose any player" — in 2-player this is always the opponent. Multi-player would need a player-selection step.

First card to use `specialActionType = IMMEDIATE` on a `UnicornCardData` subclass rather than `MagicCardData` — confirmed `CardManager.PlayCardForCurrentPlayer` checks `specialActionType` independent of `cardType`, so this needed no framework change, just overriding it in `OnEnable()` after `base.OnEnable()`.
