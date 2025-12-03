using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "CardData/MagicCardData")]
public class MagicCardData : CardData
{
    
    public override void TriggerSpecialAction()
    {
        Debug.Log("Triggered special action for MagicCardDataScriptableObject");
    }

    public override void OnEnable()
    {
        cardType = CardType.MAGIC;
        afterAction = AfterAction.DISCARD;
        specialActionType = SpecialActionType.IMMEDIATE;
    }
}
