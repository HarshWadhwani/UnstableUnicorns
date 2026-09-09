using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CardData/MagicCardData/KinkShameCardData")]
public class KinkShameCardData : MagicCardData
{
    public override void OnEnable()
    {
        base.OnEnable();

        actions = new List<CardAction>
        {
            new ChooseEffectAction
            {
                chooser = ChooseEffectAction.Chooser.ActivePlayer,

                optionALabel = "Destroy an Upgrade",
                optionA = new List<CardAction>
                {
                    new DestroyCardAction
                    {
                        destroyer = DestroyCardAction.DestroyerPlayer.ActivePlayer,
                        targetStable = DestroyCardAction.TargetStable.Upgrade,
                        numberOfCards = 1
                    }
                },
                optionAViable = ctx => ctx.opponentPlayer != null
                                       && ctx.opponentPlayer.upgradeStable.spaceCards.Count > 0,

                optionBLabel = "Sacrifice a Downgrade",
                optionB = new List<CardAction>
                {
                    new SacrificeCardAction
                    {
                        sacrificer = SacrificeCardAction.SacrificerPlayer.ActivePlayer,
                        targetStable = SacrificeCardAction.TargetStable.Downgrade,
                        sacrificeAll = false,
                        numberOfCards = 1
                    }
                },
                optionBViable = ctx => ctx.activePlayer.downgradeStable.spaceCards.Count > 0
            }
        };
    }

    public override bool CanPlay(Player activePlayer, Player opponentPlayer)
    {
        return (opponentPlayer != null && opponentPlayer.upgradeStable.spaceCards.Count > 0)
               || activePlayer.downgradeStable.spaceCards.Count > 0;
    }
}
