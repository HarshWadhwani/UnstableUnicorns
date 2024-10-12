using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NeighMeansNeighScriptableObject", menuName = "ScriptableObjects/NeighCards/NeighMeansNeigh")]
public class NeighMeansNeighScriptableObject : NeighCardScriptableObject
{
    private void OnEnable()
    {
        cardNameVariations = new List<string>
        {
            "Neigh means neigh"
        };
        cardDescriptionText = "";
        instances = 1;
        neighType = NeighType.Final;
    }

    public override void TriggerSpecialAction()
    {
        Debug.Log("Triggered special action for NeighMeansNeighScriptableObject");
    }
}
