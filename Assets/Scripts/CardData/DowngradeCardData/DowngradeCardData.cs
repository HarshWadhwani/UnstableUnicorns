using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CardData/DowngradeCardData")]
public class DowngradeCardData : CardData
{
    public override void TriggerSpecialAction()
    {
        Debug.Log("Triggered Special Action for DowngradeCardDataScriptableObjects");
    }
    
    public override void OnEnable()
    {
        cardType = CardType.DOWNGRADE;
        specialActionType = SpecialActionType.EVERY_TURN;
        afterAction = AfterAction.PLACE_IN_STABLE;
    }
}
