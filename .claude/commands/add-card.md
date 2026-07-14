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
2. **Call ordering matters.** `Deck.MoveToTop` appends the card to the end of `spaceCards`, and each subsequent call pushes its own card above the one before it. This means the Force calls in `DeckManager.Start()` are listed in *reverse draw order* — the LAST call in the list is the FIRST card drawn. Add the new card's Force call as the **last line** of the Force-call block (immediately before the baby-unicorn `foreach` draw loop) so it's the very next card drawn — that's almost always what you want when testing a card you just added.
3. **Don't break existing ordering dependencies.** A couple of Force calls are intentionally sequenced relative to each other and have inline comments saying so (e.g. Hentaicorn must be drawn and played to a stable *before* the opponent draws Horrifying Impaling, to test the sacrifice-shield interaction). If you're inserting your new call among calls that have such a comment, insert it *before* that dependent group rather than splitting it apart, and update the "drawn 1st" wording in the comment since it will no longer be literally first.
4. If the card's `CanPlay` needs board state that doesn't exist at game start (e.g. a card already sitting in a stable), still put the Force call at the top — leave a comment noting what manual setup is needed first. Being immediately drawable is more useful for testing than being buried deep in the deck, and verifying the `CanPlay` guard itself (does it correctly block the card before setup?) is part of testing too.

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

**Known recurring gotcha:** on nearly every card added so far, the `.asset` got created with `cardNameVariations: []` and `instances: 0` left at their defaults — usually because the Inspector list-add wasn't committed before saving. `instances: 0` means `DeckManager.GenerateCards` creates zero copies of the card, so it silently never enters the deck at all (including for the Force-to-top debug hook — it'll just log a "not found" warning). After the user says the asset is created, read the `.asset` file directly and check both fields before treating the card as ready to test.
