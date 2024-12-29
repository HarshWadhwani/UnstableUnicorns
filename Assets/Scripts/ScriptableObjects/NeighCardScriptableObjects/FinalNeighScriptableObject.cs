using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FinalNeighScriptableObject", menuName = "ScriptableObjects/NeighCards/FinalNeigh")]
public class FinalNeighScriptableObject : NeighCardScriptableObject
{
    public override void TriggerSpecialAction()
    {
        Debug.Log("Triggered special action for FinalNeighScriptableObject");
    }
}
