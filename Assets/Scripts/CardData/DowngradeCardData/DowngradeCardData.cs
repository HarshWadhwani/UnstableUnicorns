using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CardData/DowngradeCardData")]
public class DowngradeCardData : CardData
{

    public override void TriggerSpecialAction()
    {
        Debug.Log("Triggered special action for NeighMotherFuckerScriptableObject");
    }

    public override void OnEnable()
    {
        cardType = CardType.DOWNGRADE;
        afterAction = AfterAction.PLACE_IN_ENEMY_STABLE;
        specialActionType = SpecialActionType.EVERY_TURN;
    }
}
