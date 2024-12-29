using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BasicNeighScriptableObject", menuName = "ScriptableObjects/NeighCards/BasicNeigh")]
public class BasicNeighScriptableObject : NeighCardScriptableObject
{
    public override void TriggerSpecialAction()
    {
        Debug.Log("Triggered special action for BasicNeighScriptableObject");
    }
}
