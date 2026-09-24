using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CardData/MagicCardData/RainbowShitstormCardData")]
public class RainbowShitstormCardData : MagicCardData
{
    public override void OnEnable()
    {
        base.OnEnable();

        // "Each player (including you) must SACRIFICE a card and DISCARD their hand. Shuffle the
        // discard pile into the deck, then deal 5 cards to each player."
        // Two separate loops keep the printed order: everyone sacrifices + discards, then one
        // shuffle, then everyone is dealt 5.
        actions = new List<CardAction>
        {
            new ForEachPlayerAction
            {
                actions = new List<CardAction>
                {
                    new SacrificeCardAction
                    {
                        targetStable = SacrificeCardAction.TargetStable.Any,
                        sacrificeAll = false,
                        numberOfCards = 1
                    },
                    new DiscardHandAction()
                }
            },
            new ShuffleDiscardIntoDeckAction(),
            new ForEachPlayerAction
            {
                actions = new List<CardAction> { new DrawCardAction { numberOfCards = 5 } }
            }
        };
    }
}
