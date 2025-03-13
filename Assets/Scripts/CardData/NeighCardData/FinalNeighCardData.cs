using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CardData/NeighCardData/FinalNeighCardData")]
public class FinalNeighCardData : NeighCardData
{
    public override void TriggerSpecialAction()
    {
        Debug.Log("Triggered special action for FinalNeighScriptableObject");
    }

    public override void OnEnable()
    {
        base.OnEnable();
        neighType = NeighType.Final;
    }
}
