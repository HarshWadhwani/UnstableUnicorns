using UnityEngine;

public class CardActionContext
{
    // "You" for this action. Normally the player whose card this is; inside ForEachPlayerAction
    // it's the player the current pass is for.
    public Player activePlayer;
    public Player opponentPlayer;
    // The player who played the source card. Never rebound by ForEachPlayerAction — use it when
    // an effect inside a per-player loop still needs to mean the caster.
    public Player caster;
    public Card sourceCard;
    public TurnManager turnManager;
    public CardManager cardManager;
    public DiscardPile discardPile;
    public Deck playDeck;
    public DeckManager deckManager;

    public CardActionContext(Player activePlayer, Player opponentPlayer, Card sourceCard,
                            TurnManager turnManager, CardManager cardManager, DiscardPile discardPile,
                            Deck playDeck, DeckManager deckManager)
    {
        this.activePlayer = activePlayer;
        this.opponentPlayer = opponentPlayer;
        this.sourceCard = sourceCard;
        this.turnManager = turnManager;
        this.cardManager = cardManager;
        this.discardPile = discardPile;
        this.playDeck = playDeck;
        this.deckManager = deckManager;
        this.caster = activePlayer;
    }

    // A copy with "you" rebound to `player` (and `opponent`), everything else — including
    // caster and sourceCard — unchanged.
    public CardActionContext ForPlayer(Player player, Player opponent)
    {
        var copy = (CardActionContext)MemberwiseClone();
        copy.activePlayer = player;
        copy.opponentPlayer = opponent;
        return copy;
    }
}
