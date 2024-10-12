using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NeighMotherFuckerScriptableObject", menuName = "ScriptableObjects/NeighCards/NeighMotherfucker")]
public class NeighMotherFuckerScriptableObject : NeighCardScriptableObject
{
    private void OnEnable()
    {
        cardNameVariations = new List<string>
        {
            "Neigh, Motherfucker"
        };
        cardDescriptionText = "";
        instances = 3;
        neighType = NeighType.ForceOpponentDiscard;
    }

    public override void TriggerSpecialAction()
    {
        Debug.Log("Triggered special action for NeighMotherFuckerScriptableObject");
    }
}
