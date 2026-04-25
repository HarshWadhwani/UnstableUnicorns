using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CardData/NeighCardData/DiscardNeighCardData")]
public class DiscardNeighCardData : NeighCardData
{
    public override void OnEnable()
    {
        base.OnEnable();
        neighType = NeighType.ForceOpponentDiscard;
    }
}
