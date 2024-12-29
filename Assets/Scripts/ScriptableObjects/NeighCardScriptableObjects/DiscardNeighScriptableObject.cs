using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DiscardNeighScriptableObject", menuName = "ScriptableObjects/NeighCards/DiscardNeigh")]
public class DiscardNeighScriptableObject : NeighCardScriptableObject
{
    public override void TriggerSpecialAction()
    {
        Debug.Log("Triggered special action for DiscardNeighScriptableObject");
    }
}
