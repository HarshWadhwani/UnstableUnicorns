using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CardData/MagicCardData/SexDrugsAndUnicornsCardData")]
public class SexDrugsAndUnicornsCardData : MagicCardData
{
    public override void OnEnable()
    {
        base.OnEnable();

        // "Choose a player. That player must either DISCARD 3 cards or SACRIFICE a Unicorn card."
        // In 2-player the chosen player is always the opponent, and they make the choice.
        actions = new List<CardAction>
        {
            new ChooseEffectAction
            {
                chooser = ChooseEffectAction.Chooser.Opponent,

                optionALabel = "Discard 3 cards",
                optionA = new List<CardAction>
                {
                    new DiscardCardAction
                    {
                        targetPlayer = DiscardCardAction.TargetPlayer.Opponent,
                        selectionMode = DiscardCardAction.SelectionMode.PlayerChooses,
                        numberOfCards = 3
                    }
                },
                // Only offered when it can complete — closes the count-check trap on "discard 3".
                optionAViable = ctx => ctx.opponentPlayer != null
                                       && ctx.opponentPlayer.handStable.spaceCards.Count >= 3,

                optionBLabel = "Sacrifice a Unicorn",
                optionB = new List<CardAction>
                {
                    new SacrificeCardAction
                    {
                        sacrificer = SacrificeCardAction.SacrificerPlayer.Opponent,
                        targetStable = SacrificeCardAction.TargetStable.Unicorn,
                        sacrificeAll = false,
                        numberOfCards = 1
                    }
                },
                optionBViable = ctx => ctx.opponentPlayer != null
                                       && ctx.opponentPlayer.unicornStable.spaceCards.Count > 0
            }
        };
    }

    public override bool CanPlay(Player activePlayer, Player opponentPlayer)
    {
        return opponentPlayer != null
               && (opponentPlayer.handStable.spaceCards.Count >= 3
                   || opponentPlayer.unicornStable.spaceCards.Count > 0);
    }
}
