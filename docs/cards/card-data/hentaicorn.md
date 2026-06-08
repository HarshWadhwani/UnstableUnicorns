# Hentaicorn

| Field | Value |
|-------|-------|
| wiki_type | Magical Unicorn |
| card_type | UNICORN |
| unicorn_subtype | MAGIC |
| copies | 1 |
| trigger | NONE |
| can_play | always |
| impl_status | done |
| impl_class | HentaicornCardData.cs |

## Effect (2nd Edition)
> "If a Unicorn card in your Stable would be destroyed, you may SACRIFICE this card instead."

## Action Mapping
NEW: passive ability — intercepts a destroy targeting any unicorn in your stable; you may sacrifice this card to prevent it.

## Passive Interfaces
ISacrificeShield (card auto-sacrifices to intercept a destroy)
