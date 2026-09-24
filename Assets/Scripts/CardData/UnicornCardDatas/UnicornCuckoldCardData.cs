using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CardData/UnicornCardData/UnicornCuckoldCardData")]
public class UnicornCuckoldCardData : UnicornCardData
{
    private MoveUnicornAction loan;

    public override void OnEnable()
    {
        base.OnEnable();
        unicornType = UnicornType.MAGIC;
        specialActionType = SpecialActionType.EVERY_TURN;

        // "If this card is in your Stable at the beginning of your turn, move a Unicorn card from
        // your Stable into any other player's Stable. At the end of your turn, return that Unicorn
        // card to your Stable."
        loan = new MoveUnicornAction { source = MoveUnicornAction.Source.Self, returnAtEndOfTurn = true };
        actions = new List<CardAction> { loan };
    }

    public override bool CanActivateEveryTurn(Player activePlayer, Player opponentPlayer)
    {
        CardActionExecutor executor = CardActionExecutor.Instance;
        return executor != null && loan.EligibleSources(executor, activePlayer).Count > 0;
    }
}
