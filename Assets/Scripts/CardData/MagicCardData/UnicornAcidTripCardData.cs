using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CardData/MagicCardData/UnicornAcidTripCardData")]
public class UnicornAcidTripCardData : MagicCardData
{
    public override void OnEnable()
    {
        base.OnEnable();

        // "Each player (including you) must DISCARD their hand and DRAW the same number of cards
        // they discarded. You may DRAW an additional 2 cards."
        actions = new List<CardAction>
        {
            new ForEachPlayerAction
            {
                actions = new List<CardAction> { new DiscardHandAction { redrawSameCount = true } }
            },
            // Outside the loop, so "you" is the caster again.
            new ChooseEffectAction
            {
                chooser = ChooseEffectAction.Chooser.ActivePlayer,
                optionALabel = "Draw 2 more",
                optionA = new List<CardAction> { new DrawCardAction { numberOfCards = 2 } },
                optionAViable = ctx => ctx.playDeck.spaceCards.Count > 0,
                optionBLabel = "No thanks",
                optionB = new List<CardAction>()
            }
        };
    }
}
