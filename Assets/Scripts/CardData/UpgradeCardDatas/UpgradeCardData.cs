using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CardData/UpgradeCardData")]
public class UpgradeCardData : CardData
{
    public override void OnEnable()
    {
        cardType = CardType.UPGRADE;
        specialActionType = SpecialActionType.EVERY_TURN;
    }
}
