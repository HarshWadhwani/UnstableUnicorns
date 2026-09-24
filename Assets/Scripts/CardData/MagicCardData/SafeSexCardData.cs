using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CardData/MagicCardData/SafeSexCardData")]
public class SafeSexCardData : MagicCardData
{
    public override void OnEnable()
    {
        base.OnEnable();

        // "Each player (including you) must return a Baby Unicorn card from their Stable to the Nursery."
        actions = new List<CardAction>
        {
            new ForEachPlayerAction
            {
                actions = new List<CardAction> { new ReturnBabyToNurseryAction() }
            }
        };
    }
}
