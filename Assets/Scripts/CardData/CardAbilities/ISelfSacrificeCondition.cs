// Implemented by any CardData that must be sacrificed as soon as a board condition holds
// ("If at any time ..., SACRIFICE this card"). CardManager.MoveCard re-checks every card in
// every stable after each move — every change to the board goes through MoveCard — and
// sacrifices the ones whose condition is true. `owner` is the player whose stable the card is in.
public interface ISelfSacrificeCondition
{
    bool ShouldSacrifice(Player owner);
}
