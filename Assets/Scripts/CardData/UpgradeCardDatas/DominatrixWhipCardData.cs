using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CardData/UpgradeCardData/DominatrixWhipCardData")]
public class DominatrixWhipCardData : UpgradeCardData
{
    private MoveUnicornAction move;

    public override void OnEnable()
    {
        base.OnEnable();

        // "If this card is in your Stable at the beginning of your turn, you may move a Unicorn
        // card from any player's Stable to any other player's Stable. You cannot move that card to
        // your own Stable." With 2 players that means one of your Unicorns to your opponent.
        move = new MoveUnicornAction { source = MoveUnicornAction.Source.AnyPlayer };
        actions = new List<CardAction> { move };
    }

    public override bool CanActivateEveryTurn(Player activePlayer, Player opponentPlayer)
    {
        CardActionExecutor executor = CardActionExecutor.Instance;
        return executor != null && move.EligibleSources(executor, activePlayer).Count > 0;
    }
}
