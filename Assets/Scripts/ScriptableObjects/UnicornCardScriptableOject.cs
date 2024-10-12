using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UnicornCardSO", menuName = "ScriptableObjects/UnicornCardSO")]
public class UnicornCardScriptableOject : CardScriptableObject
{
    public UnicornType unicornType;

    public override void TriggerSpecialAction()
    {
        Debug.Log("Triggered special action for NeighMotherFuckerScriptableObject");
    }
}
