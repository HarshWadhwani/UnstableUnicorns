using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CardData/NeighCardData/BasicNeighCardData")]
public class BasicNeighCardData : NeighCardData
{
    public override void TriggerSpecialAction()
    {
        Debug.Log("Triggered special action for BasicNeighScriptableObject");
    }

    public override void OnEnable()
    {
        base.OnEnable();
    }
}
