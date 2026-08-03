using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CardData/MagicCardData/BlazeAndGrazeCardData")]
public class BlazeAndGrazeCardData : MagicCardData
{
    public override void OnEnable()
    {
        base.OnEnable();

        actions = new List<CardAction>
        {
            new RevealTopDeckAction()
        };
    }
}
