using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CardData/DowngradeCardData/UnicornFlatulenceCardData")]
public class UnicornFlatulenceCardData : DowngradeCardData, ISelfSacrificeCondition
{
    public override void OnEnable()
    {
        base.OnEnable();

        // "If this card is in your Stable at the beginning of your turn, return a Unicorn card
        // from your Stable to your hand." Mandatory (Downgrade).
        actions = new List<CardAction>
        {
            new ReturnToHandAction
            {
                whose = ReturnToHandAction.Whose.Self,
                targetStable = ReturnToHandAction.TargetStable.Unicorn
            }
        };
    }

    // "If at any time you have no Unicorn cards in your Stable, SACRIFICE this card."
    public bool ShouldSacrifice(Player owner) => owner.unicornStable.spaceCards.Count == 0;
}
