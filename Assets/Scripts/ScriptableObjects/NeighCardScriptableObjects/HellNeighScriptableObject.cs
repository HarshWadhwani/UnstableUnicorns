using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HellNeighScriptableObject", menuName = "ScriptableObjects/NeighCards/HellNeigh")]
public class HellNeighScriptableObject : NeighCardScriptableObject
{
    private void OnEnable()
    {
        cardNameVariations = new List<string>
        {
            "Hell Neigh",
            "Neigh, Bitch",
            "The Safeword is Neigh"
        };
        cardDescriptionText = "";
        instances = 11;
        neighType = NeighType.Basic;
    }

    public override void TriggerSpecialAction()
    {
        Debug.Log("Triggered special action for HellNeighScriptableObject");
    }
}
