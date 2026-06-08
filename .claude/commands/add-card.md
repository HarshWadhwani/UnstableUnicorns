Read `docs/cards/card-implementation-guide.md` first.

## Step 1 — Look up pre-extracted card data

Check `docs/cards/card-data/_checklist.md` for the card named in $ARGUMENTS.

If a matching row exists (Extracted ✅), read its card data file (e.g. `docs/cards/card-data/hentaicorn.md`).
The file contains: canonical effect text, copy count, trigger type, action mapping, CanPlay conditions, passive interfaces, and rulings.
Use this as your primary source — it saves re-deriving everything from scratch and ensures 2nd Edition wording is used.

If the card is not in the checklist, derive everything from the description in $ARGUMENTS as usual.

## Step 2 — Implement the card

Work through the guide's decisions in order:
1. Determine CardType (from the data file's `card_type` field, or from the description)
2. Note the SpecialActionType — confirm the `trigger` field matches the effect timing
3. Map each effect step to a CardAction — the data file's **Action Mapping** section has this pre-done; use it directly
4. Check the **Passive Interfaces** section — implement any listed interface (e.g. `ISacrificeShield`)
5. Check the **CanPlay Override** section — add `CanPlay()` if listed
6. Decide whether a new C# subclass is needed or an existing base class suffices
7. Set `instances` from the data file's `copies` field

If a new C# class is needed, write it following the template in the guide.

If the Action Mapping contains `NEW: ...` entries, those action types don't exist yet — flag them to the user before writing any code and do not proceed until the user decides how to handle them.

## Step 3 — Add debug stacking to DeckManager

Once the card's C# class exists, add a debug stacking method to `DeckManager.cs`:
1. Add a private `Force<CardNameNoSpaces>ToTop()` method — find the card in `playDeck.spaceCards` by its CardData type, call `playDeck.MoveToTop(card)`.
2. Call it from `DeckManager.Start()` after the existing Force* calls.

## Step 4 — Update the card data file and checklist

After the code is written:
1. In the card's data file (e.g. `docs/cards/card-data/hentaicorn.md`), set:
   - `impl_status` → `done`
   - `impl_class` → the C# filename (e.g. `HentaicornCardData.cs`)
2. In `docs/cards/card-data/_checklist.md`, change the card's **Implemented** column from ⬜ to ✅ and update the Summary count.

## Step 5 — Tell the user

- What `.asset` to create in Unity, where to save it, and what fields to fill in (`cardNameVariations`, `cardDescriptionText`, `instances`)
- Any `NEW:` action types that blocked implementation (if any)
- A short testing checklist for the card's effect in Play mode
